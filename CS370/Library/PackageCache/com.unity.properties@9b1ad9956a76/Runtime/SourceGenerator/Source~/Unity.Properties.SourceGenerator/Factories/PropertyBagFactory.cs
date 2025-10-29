using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Unity.Properties.SourceGenerator
{
    /// <summary>
    /// The <see cref="PropertyBagFactory"/> is used to construct a proper class declaration for a given property bag. It contains all the necessary logic for writing code syntax blocks.
    /// </summary>
    static class PropertyBagFactory
    {
        public static ClassDeclarationSyntax CreatePropertyBagClassDeclarationSyntax(PropertyBagDefinition propertyBag)
        {
            var builder = new StringBuilder();

            builder.AppendLine("[global::System.Runtime.CompilerServices.CompilerGenerated]");
            builder.AppendLine($"sealed class {propertyBag.PropertyBagClassName} : global::Unity.Properties.ContainerPropertyBag<{propertyBag.ContainerType.GetGlobalSanitizedName()}>");
            builder.AppendLine("{");

            builder.AppendLine($"public {propertyBag.PropertyBagClassName}()");
            builder.AppendLine("{");
            foreach (var property in propertyBag.GetValidPublicPropertyMembers())
            {
                builder.Append($"AddProperty(new ").Append(property.PropertyClassName).AppendLine("());");
            }

            if (propertyBag.GetValidNonPublicPropertyMembers().Any())
            {
                foreach (var property in propertyBag.GetValidNonPublicPropertyMembers())
                {
                    builder.Append($"AddProperty(new ").Append(property.PropertyClassName).AppendLine("());");
                }
            }

            foreach (var registerCollectionTypeMethod in GetRegisterCollectionTypeMethod(propertyBag))
                builder.AppendLine(registerCollectionTypeMethod);

            builder.Append("}");

            foreach (var property in propertyBag.GetValidPublicPropertyMembers())
            {
                builder.AppendLine("[global::System.Runtime.CompilerServices.CompilerGenerated]");
                builder.AppendLine($"class {property.PropertyClassName} : global::Unity.Properties.Property<{propertyBag.ContainerType.GetGlobalSanitizedName()}, {property.PropertyTypeName}>");
                builder.AppendLine("{");
                builder.AppendLine($"public override string Name => \"{property.PropertyName}\";");
                builder.AppendLine($"public override bool IsReadOnly => {property.IsReadOnly.ToString().ToLower()};");

                builder.AppendLine($"public override {property.PropertyTypeName} GetValue(ref {propertyBag.ContainerType.GetGlobalSanitizedName()} container) => container.{property.PropertyName};");

                builder.AppendLine(property.IsReadOnly
                    ? $"public override void SetValue(ref {propertyBag.ContainerType.GetGlobalSanitizedName()} container, {property.PropertyTypeName} value) => throw new System.InvalidOperationException(\"Property is ReadOnly\");"
                    : $"public override void SetValue(ref {propertyBag.ContainerType.GetGlobalSanitizedName()} container, {property.PropertyTypeName} value) => container.{property.PropertyName} = value;");

                var isNamedTuple = property.MemberType.IsTupleType && property.MemberType.GetMembers().OfType<IFieldSymbol>().Any(f => !SymbolEqualityComparer.Default.Equals(f.CorrespondingTupleField, f));
                if (property.HasCustomAttributes || isNamedTuple)
                {
                    builder.AppendLine($"public {property.PropertyClassName}()");
                    builder.AppendLine("{");
                    if (property.HasCustomAttributes)
                    {
                        var bindingFlagAccessibility = property.DeclaredAccessibility == Accessibility.Public ? "Public" : "NonPublic";
                        if (property.IsField)
                        {
                            builder.AppendLine($"AddAttributes(typeof({propertyBag.ContainerType.GetGlobalSanitizedName()}).GetField(\"{property.MemberName}\", BindingFlags.Instance | BindingFlags.{bindingFlagAccessibility}).GetCustomAttributes());");
                        }
                        else
                        {
                            builder.AppendLine($"var properties = typeof({propertyBag.ContainerType.GetGlobalSanitizedName()}).GetProperties(BindingFlags.Instance | BindingFlags.{bindingFlagAccessibility});");
                            builder.AppendLine("foreach (var property in properties)");
                            builder.AppendLine("{");
                            builder.AppendLine($"if (property.Name != \"{property.MemberName}\" || property.DeclaringType != typeof({property.DeclaringType.GetGlobalSanitizedName()})) continue;");
                            builder.AppendLine("AddAttributes(property.GetCustomAttributes());");
                            builder.AppendLine("return;");
                            builder.AppendLine("}");
                        }
                    }

                    if (isNamedTuple)
                    {
                        builder.Append("AddAttribute(new global::System.Runtime.CompilerServices.TupleElementNamesAttribute(new string[] {");

                        var tupleFields = property.MemberType.GetMembers().OfType<IFieldSymbol>().ToArray();

                        for (var i = 0; i < tupleFields.Length; ++i)
                        {
                            var field = tupleFields[i];
                            // Named tuple element
                            if (!SymbolEqualityComparer.Default.Equals(field.CorrespondingTupleField, field))
                            {
                                builder.Append("\"");
                                builder.Append(field.Name);
                                builder.Append("\"");
                                if (i < tupleFields.Length - 1)
                                {
                                    builder.Append(", ");
                                }
                            }
                            // Unnamed tuple element
                            else
                            {
                                // Named tuple elements are preceded by their unnamed counterpart, so we need to do a look-up
                                if (i + 1 < tupleFields.Length &&
                                    !SymbolEqualityComparer.Default.Equals(tupleFields[i + 1].CorrespondingTupleField, tupleFields[i + 1]))
                                    continue;

                                builder.Append("null");
                                if (i < tupleFields.Length - 1)
                                {
                                    builder.Append(", ");
                                }
                            }
                        }

                        builder.AppendLine("}));");
                    }

                    builder.AppendLine("}");
                }

                builder.AppendLine("}");
            }

            if (propertyBag.GetValidNonPublicPropertyMembers().Any())
            {
                foreach (var property in propertyBag.GetValidNonPublicPropertyMembers())
                {
                    var bindingFlagAccessibility = property.DeclaredAccessibility == Accessibility.Public ? "Public" : "NonPublic";

                    var getMemberMethod = property.IsField ? "GetField" : "GetProperty";
                    builder.AppendLine("[global::System.Runtime.CompilerServices.CompilerGenerated]");
                    builder.AppendLine($"class {property.PropertyClassName} : global::Unity.Properties.ReflectedMemberProperty<{propertyBag.ContainerType.GetGlobalSanitizedName()}, {property.PropertyTypeName}>");
                    builder.AppendLine("{");
                    builder.AppendLine($"    public {property.PropertyClassName}()");
                    builder.AppendLine($"        : base(typeof({property.MemberSymbol.ContainingType}).{getMemberMethod}(\"{property.MemberName}\", BindingFlags.Instance | BindingFlags.{bindingFlagAccessibility}), \"{property.PropertyName}\")");
                    builder.AppendLine("    {");
                    builder.AppendLine("    }");
                    builder.AppendLine("}");
                }
            }

            builder.AppendLine($"}}");

            var propertyBagClassDeclarationSyntax = SyntaxFactory.ParseMemberDeclaration(builder.ToString()) as ClassDeclarationSyntax;

            if (null == propertyBagClassDeclarationSyntax)
                throw new Exception($"Failed to construct ClassDeclarationSyntax for ContainerType=[{propertyBag.ContainerTypeName}]");

            return propertyBagClassDeclarationSyntax;
        }

        static IEnumerable<string> GetRegisterCollectionTypeMethod(PropertyBagDefinition propertyBag)
        {
            var visited = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);

            foreach (var property in propertyBag.GetPropertyMembers())
            foreach (var line in GetRegisterCollectionTypesRecurse(propertyBag.ContainerType, property.MemberType, visited))
                yield return line;
        }

        static IEnumerable<string> GetRegisterCollectionTypesRecurse(ITypeSymbol containerType, ITypeSymbol propertyType, HashSet<ITypeSymbol> visited)
        {
            if (!visited.Add(propertyType)) yield break;

            switch (propertyType)
            {
                case IArrayTypeSymbol arrayTypeSymbol:
                {
                    yield return $"global::Unity.Properties.PropertyBag.RegisterArray<{containerType.GetGlobalSanitizedName()}, {arrayTypeSymbol.ElementType.GetGlobalSanitizedName()}>();";

                    foreach (var inner in GetRegisterCollectionTypesRecurse(arrayTypeSymbol,
                        arrayTypeSymbol.ElementType, visited))
                        yield return inner;

                    break;
                }
                case INamedTypeSymbol namedTypeSymbol:
                {
                    if (propertyType.Interfaces.Any(i => i.MetadataName == "IList`1"))
                    {
                        var elementTypeSymbol = propertyType.Interfaces.First(i => i.MetadataName == "IList`1").TypeArguments[0];

                        if (propertyType.MetadataName == "List`1")
                            yield return $"global::Unity.Properties.PropertyBag.RegisterList<{containerType.GetGlobalSanitizedName()}, {elementTypeSymbol.GetGlobalSanitizedName()}>();";
                        else
                            yield return $"global::Unity.Properties.PropertyBag.RegisterIList<{containerType.GetGlobalSanitizedName()}, {namedTypeSymbol.GetGlobalSanitizedName()}, {elementTypeSymbol.GetGlobalSanitizedName()}>();";

                        foreach (var inner in GetRegisterCollectionTypesRecurse(namedTypeSymbol, elementTypeSymbol, visited))
                            yield return inner;
                    }
                    else if (propertyType.Interfaces.Any(i => i.MetadataName == "ISet`1"))
                    {
                        var elementTypeSymbol = propertyType.Interfaces.First(i => i.MetadataName == "ISet`1").TypeArguments[0];

                        if (propertyType.MetadataName == "HashSet`1")
                            yield return $"global::Unity.Properties.PropertyBag.RegisterHashSet<{containerType.GetGlobalSanitizedName()}, {elementTypeSymbol.GetGlobalSanitizedName()}>();";
                        else
                            yield return $"global::Unity.Properties.PropertyBag.RegisterISet<{containerType.GetGlobalSanitizedName()}, {namedTypeSymbol.GetGlobalSanitizedName()}, {elementTypeSymbol.GetGlobalSanitizedName()}>();";

                        foreach (var inner in GetRegisterCollectionTypesRecurse(namedTypeSymbol, elementTypeSymbol, visited))
                            yield return inner;
                    }
                    else if (propertyType.Interfaces.Any(i => i.MetadataName == "IDictionary`2"))
                    {
                        var interfaceSymbol = propertyType.Interfaces.First(i => i.MetadataName == "IDictionary`2");

                        var keyTypeSymbol = interfaceSymbol.TypeArguments[0];
                        var valueTypeSymbol = interfaceSymbol.TypeArguments[1];

                        if (propertyType.MetadataName == "Dictionary`2")
                            yield return $"global::Unity.Properties.PropertyBag.RegisterDictionary<{containerType.GetGlobalSanitizedName()}, {keyTypeSymbol.GetGlobalSanitizedName()}, {valueTypeSymbol.GetGlobalSanitizedName()}>();";
                        else
                            yield return $"global::Unity.Properties.PropertyBag.RegisterIDictionary<{containerType.GetGlobalSanitizedName()}, {namedTypeSymbol.GetGlobalSanitizedName()}, {keyTypeSymbol.GetGlobalSanitizedName()}, {valueTypeSymbol.GetGlobalSanitizedName()}>();";

                        foreach (var inner in GetRegisterCollectionTypesRecurse(namedTypeSymbol, keyTypeSymbol, visited))
                            yield return inner;

                        foreach (var inner in GetRegisterCollectionTypesRecurse(namedTypeSymbol, valueTypeSymbol, visited))
                            yield return inner;
                    }

                    break;
                }
            }
        }
    }
}
