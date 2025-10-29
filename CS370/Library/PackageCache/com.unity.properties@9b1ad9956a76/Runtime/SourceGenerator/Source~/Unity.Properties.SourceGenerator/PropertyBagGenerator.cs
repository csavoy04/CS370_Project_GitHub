using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Unity.Properties.SourceGenerator
{
    /// <summary>
    /// The <see cref="PropertyBagGenerator"/> is the entry point for code generation. This generator does not work with syntax but rather exclusively with the symbols.
    ///
    /// The generator work as follows:
    ///     1) Using the compilation we gather all type symbols which should have property bags generated based on the following attributes:
    ///         * <see cref="Unity.Properties.GeneratePropertyBagAttribute"/>
    ///         * <see cref="Unity.Properties.GeneratePropertyBagsForTypesQualifiedWithAttribute"/>
    ///         * <see cref="Unity.Properties.GeneratePropertyBagsForTypeAttribute"/>
    ///
    ///     2) The type symbols are then parsed and a packed in to a <see cref="PropertyBagDefinition"/> model.
    ///     3) The <see cref="PropertyBagDefinition"/> model is then passed to the <see cref="PropertyBagFactory"/> which handles writing out <see cref="ClassDeclarationSyntax"/> objects.
    ///     4) The generated syntax objects are then written out properly formatted code.
    ///
    /// The generator does not maintain any state; this means we can produce duplicate property bag code across multiple assemblies. This can happen if a container type references a type from another assembly.
    /// </summary>
    /// <remarks>
    /// See https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/work-with-semantics for a primer on working with roslyn.
    /// </remarks>
    [Generator]
    public class PropertyBagGenerator : ISourceGenerator
    {
        // Entry assembly is null in VS IDE
        // At build time, it is `csc` or `VBCSCompiler`
        private static readonly bool IsBuildTime = Assembly.GetEntryAssembly() != null;

        /// <summary>
        /// Called before generation occurs. A generator can use the context to register callbacks required to perform generation.
        /// </summary>
        /// <param name="context">The context which can be used to register a set of callbacks.</param>
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new OptInCodeGenerationSyntaxReceiver());

            // Don't run if not in buildtime
            if (!IsBuildTime)
                return;
        }

        /// <summary>
        /// Called to perform source generation. A generator can use the context to add source files via the AddSource(String, SourceText) method.
        /// </summary>
        /// <param name="context">The context which provides access to the current compilation and allows manipulation of the output.</param>
        public void Execute(GeneratorExecutionContext context)
        {
            // Don't run if not in build time
            if (!IsBuildTime)
                return;

            var syntaxReceiver = (OptInCodeGenerationSyntaxReceiver) context.SyntaxReceiver;

            var shouldRun = false;
            if (null != syntaxReceiver)
            {
                foreach (var attribute in syntaxReceiver.Attributes)
                {
                    var semanticModel = context.Compilation.GetSemanticModel(attribute.SyntaxTree);
                    if (semanticModel.GetTypeInfo(attribute).Type?.GetGlobalSanitizedName() == "global::Unity.Properties.GeneratePropertyBagsForAssemblyAttribute")
                    {
                        shouldRun = true;
                        break;
                    }
                }
            }

            if (!shouldRun)
                return;

            try
            {
                context.CancellationToken.ThrowIfCancellationRequested();
                
                context.DeleteGeneratedDebugSourceFolder();
                context.LogInfo($"Starting source generation for Assembly=[{context.Compilation.Assembly.Name}]");
                var stopwatch = Stopwatch.StartNew();

                // Scan through the assembly and gather all types which should have property bags generated.
                var containerTypeSymbols = ContainerTypeUtility.GetPropertyContainerTypes(context.Compilation.Assembly, context.CancellationToken);
                var propertyBags = containerTypeSymbols.Select(containerTypeSymbol => new PropertyBagDefinition(containerTypeSymbol)).ToList();

                if (propertyBags.Count != 0)
                {
                    var namespaceDeclarationSyntax = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("Unity.Properties.Generated"));
                    var usingSystemReflectionSyntax = SyntaxFactory
                        .UsingDirective(SyntaxFactory.ParseName("System.Reflection"));

                    foreach (var propertyBag in propertyBags)
                    {
                        context.CancellationToken.ThrowIfCancellationRequested();
                        
                        if (!propertyBag.IsValidPropertyBag)
                        {
                            var rule = new DiagnosticDescriptor(
                                "SGP002",
                                "Unable to generate PropertyBag",
                                $"Unable to generate PropertyBag for Type=[{propertyBag.ContainerType}]. The type is inaccessible due to its protection level. The type must be 'public', 'internal' or the containing type must be `partial`",
                                "Source Generator",
                                DiagnosticSeverity.Warning,
                                true,
                                string.Empty);

                            context.ReportDiagnostic(Diagnostic.Create(rule, propertyBag.ContainerType.GetSyntaxLocation()));
                            continue;
                        }

                        foreach (var property in propertyBag.GetPropertyMembers().Where(p => !p.IsValidProperty))
                        {
                            var rule = new DiagnosticDescriptor(
                                "SGP003",
                                "Unable to generate Property",
                                $"Unable to generate Property=[{property.PropertyName}] with Type=[{property.MemberType}] for Container=[{propertyBag.ContainerType}]. The member type is inaccessible due to its protection level. The member type must be flagged as 'public' or 'internal.",
                                "Source Generator",
                                DiagnosticSeverity.Warning,
                                true,
                                string.Empty);

                            context.ReportDiagnostic(Diagnostic.Create(rule, propertyBag.ContainerType.GetSyntaxLocation()));
                        }

                        foreach (var diagnostic in propertyBag.GetDiagnostics())
                            context.ReportDiagnostic(Diagnostic.Create(diagnostic, propertyBag.ContainerType.GetSyntaxLocation()));

                        var propertyBagDeclarationSyntax = PropertyBagFactory.CreatePropertyBagClassDeclarationSyntax(propertyBag);
                        var propertyBagCompilationUnitSyntax = SyntaxFactory.CompilationUnit();

                        if (propertyBag.UsesReflection)
                            propertyBagCompilationUnitSyntax = propertyBagCompilationUnitSyntax.AddUsings(usingSystemReflectionSyntax);

                        if (propertyBag.ContainingTypesArePartial || null == propertyBag.ContainerType.ContainingType && propertyBag.ContainerType.IsPartial())
                        {
                            var builder = new StringBuilder();
                            builder.Append($"internal static void Register{propertyBag.PropertyBagClassName}()");
                            builder.AppendLine($"{{");
                            if (propertyBag.ContainerType.IsPartial())
                                builder.AppendLine($"global::Unity.Properties.PropertyBag.Register(new {propertyBag.ContainerType.GetGlobalSanitizedName()}.{propertyBag.PropertyBagClassName}());");
                            else
                                builder.AppendLine($"global::Unity.Properties.PropertyBag.Register(new {propertyBag.ContainerType.ContainingType.GetGlobalSanitizedName()}.{propertyBag.PropertyBagClassName}());");

                            builder.AppendLine($"}}");

                            var method = SyntaxFactory.ParseMemberDeclaration(builder.ToString()) as MethodDeclarationSyntax;

                            var topLevelType = CreateWithContainingTypes(
                                propertyBag.ContainerType.IsPartial()
                                    ? propertyBag.ContainerType
                                    : propertyBag.ContainerType.ContainingType,
                                propertyBagDeclarationSyntax, method, propertyBag.PropertyBagClassName);

                            if (propertyBag.ContainerType.ContainingNamespace.IsGlobalNamespace)
                            {
                                propertyBagCompilationUnitSyntax = propertyBagCompilationUnitSyntax.AddMembers(topLevelType);
                            }
                            else
                            {
                                var containingNamespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(propertyBag.ContainerType.ContainingNamespace.ToDisplayString()));
                                containingNamespace = containingNamespace.AddMembers(topLevelType);
                                propertyBagCompilationUnitSyntax = propertyBagCompilationUnitSyntax.AddMembers(containingNamespace);
                            }
                        }
                        else
                        {
                            propertyBagCompilationUnitSyntax = propertyBagCompilationUnitSyntax.AddMembers(namespaceDeclarationSyntax.AddMembers(propertyBagDeclarationSyntax));
                        }

                        propertyBagCompilationUnitSyntax = propertyBagCompilationUnitSyntax.NormalizeWhitespace();

                        var propertyBagHint = propertyBag.LogOutputFileName;
                        var propertyBagSourceText = propertyBagCompilationUnitSyntax.GetTextUtf8();
                        context.AddSource(propertyBag.PropertyBagClassName, propertyBagSourceText);

                        if (LogOptions.LogType.HasFlag(LogType.PropertyBags))
                        {
                            var propertyBagPath = context.GetGeneratedDebugSourcePath(propertyBagHint);
                            if (!string.IsNullOrEmpty(propertyBagPath))
                                File.WriteAllText(propertyBagPath, propertyBagSourceText);
                        }
                    }

                    var registryDeclarationSyntax = PropertyBagRegistryFactory.CreatePropertyBagRegistryClassDeclarationSyntax(propertyBags);
                    var registryCompilationUnitSyntax = SyntaxFactory.CompilationUnit();

                    registryCompilationUnitSyntax = registryCompilationUnitSyntax.AddMembers(namespaceDeclarationSyntax.AddMembers(registryDeclarationSyntax));
                    registryCompilationUnitSyntax = registryCompilationUnitSyntax.NormalizeWhitespace();

                    var propertyBagRegistryHint = "PropertyBagRegistry";
                    var propertyBagRegistrySourceText = registryCompilationUnitSyntax.GetTextUtf8();
                    context.AddSource(propertyBagRegistryHint, propertyBagRegistrySourceText);

                    if (LogOptions.LogType.HasFlag(LogType.PropertyBagRegistry))
                    {
                        var propertyBagRegistryPath = context.GetGeneratedDebugSourcePath(propertyBagRegistryHint);
                        if (!string.IsNullOrEmpty(propertyBagRegistryPath))
                            File.WriteAllText(propertyBagRegistryPath, propertyBagRegistrySourceText);
                    }
                }

                context.LogInfo(
                    $"Finished source generation for Assembly=[{context.Compilation.Assembly.Name}] with {propertyBags.Count} property bags in {stopwatch.ElapsedMilliseconds}ms");
            }
            catch (Exception exception)
            {
                if (exception is OperationCanceledException)
                    throw;

                var rule = new DiagnosticDescriptor(
                    "SGP001",
                    "Unknown Exception",
                    exception.ToString(),
                    "Source Generator",
                    DiagnosticSeverity.Error,
                    true,
                    string.Empty);

                context.ReportDiagnostic(Diagnostic.Create(rule, context.Compilation.SyntaxTrees.First().GetRoot().GetLocation()));
            }
        }

        public static TypeDeclarationSyntax CreateWithContainingTypes(
            ITypeSymbol symbol,
            ClassDeclarationSyntax addToLeaf,
            MethodDeclarationSyntax method,
            string propertyBagClassName)
        {
            TypeDeclarationSyntax typeDeclaration = null;
            if (symbol.IsValueType)
            {
                typeDeclaration = SyntaxFactory.StructDeclaration(symbol.Name);
            }
            else
            {
                typeDeclaration = SyntaxFactory.ClassDeclaration(symbol.Name);
            }

            if (symbol.IsPartial())
            {
                typeDeclaration = typeDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));
            }

            typeDeclaration = typeDeclaration.AddMembers(method, addToLeaf);

            var parent = CreateWithContainingTypes(symbol.ContainingType, symbol, propertyBagClassName, typeDeclaration);
            return parent ?? typeDeclaration;
        }

        public static TypeDeclarationSyntax CreateWithContainingTypes(
            ITypeSymbol symbol,
            ITypeSymbol previous,
            string propertyBagClassName,
            TypeDeclarationSyntax previousSyntax)
        {
            if (symbol == null)
                return null;

            TypeDeclarationSyntax typeDeclaration = null;
            if (symbol.IsValueType)
                typeDeclaration = SyntaxFactory.StructDeclaration(symbol.Name);
            else
                typeDeclaration = SyntaxFactory.ClassDeclaration(symbol.Name);

            if (symbol.IsPartial())
                typeDeclaration = typeDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));

            var builder = new StringBuilder();
            builder.Append($"internal static void Register{propertyBagClassName}()");
            builder.AppendLine($"{{");
            builder.AppendLine($"{previous.GetGlobalSanitizedName()}.Register{propertyBagClassName}();");
            builder.AppendLine($"}}");

            var method = SyntaxFactory.ParseMemberDeclaration(builder.ToString()) as MethodDeclarationSyntax;

            typeDeclaration = typeDeclaration.AddMembers(method, previousSyntax);

            var parent = CreateWithContainingTypes(symbol.ContainingType, symbol, propertyBagClassName, typeDeclaration);
            return parent ?? typeDeclaration;
        }
    }
}
