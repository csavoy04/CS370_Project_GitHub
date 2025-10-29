using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Unity.Properties.SourceGenerator
{
    /// <summary>
    /// The <see cref="PropertyBagDefinition"/> class is used as a declaration for the code generator. It is used to gather and pass around all the necessary information required to output a generated property bag class.
    /// </summary>
    class PropertyBagDefinition
    {
        readonly PropertyMemberDefinition[] m_Properties;
        readonly bool m_ContainerTypeHasPrivateAccessibility;

        List<DiagnosticDescriptor> m_DiagnosticDescriptors;

        /// <summary>
        /// Gets the container type symbol for this property bag.
        /// </summary>
        public ITypeSymbol ContainerType { get; }

        /// <summary>
        /// Gets the class definition name for the generated property bag.
        /// </summary>
        public string PropertyBagClassName { get; }

        /// <summary>
        /// Gets the container type name for this property bag.
        /// </summary>
        public string ContainerTypeName { get; }

        /// <summary>
        /// Gets the filename to use when outputting the property bags to disk.
        /// </summary>
        public string LogOutputFileName { get; }

        /// <summary>
        /// Gets whether the current type is accessible through partial types.
        /// </summary>
        public bool ContainingTypesArePartial { get; }

        /// <summary>
        /// Returns the set of property member definitions.
        /// </summary>
        public IEnumerable<PropertyMemberDefinition> GetPropertyMembers() => m_Properties;

        /// <summary>
        /// Returns the set of public property member definitions.
        /// </summary>
        public IEnumerable<PropertyMemberDefinition> GetValidPublicPropertyMembers() => m_Properties
            .Where(p => p.IsValidProperty)
            .Where(p => p.IsMemberAccessible);

        /// <summary>
        /// Returns the set of non-public property member definitions.
        /// </summary>
        public IEnumerable<PropertyMemberDefinition> GetValidNonPublicPropertyMembers() => m_Properties
            .Where(p => p.IsValidProperty)
            .Where(p => !p.IsMemberAccessible);

        /// <summary>
        /// Returns true if the given property bag requires reflection to access certain properties or attributes.
        /// </summary>
        public bool UsesReflection => GetValidNonPublicPropertyMembers().Any() ||
                                      GetValidPublicPropertyMembers().Any(p => p.HasCustomAttributes);

        /// <summary>
        /// Returns <see langword="true"/> is this is a valid property bag definition.
        /// </summary>
        public bool IsValidPropertyBag => !m_ContainerTypeHasPrivateAccessibility || ContainingTypesArePartial;

        /// <summary>
        /// Constructs a new instance of <see cref="PropertyBagDefinition"/> based on the specified container type.
        /// </summary>
        /// <param name="containerType">The container type.</param>
        public PropertyBagDefinition(ITypeSymbol containerType)
        {
            ContainerType = containerType;
            m_Properties = CreatePropertyMembers(containerType).ToArray();
            LogOutputFileName = GetGeneratedPropertyBagNameOutputFilename(containerType);
            PropertyBagClassName = $"{containerType.Name}_{Guid.NewGuid():N}_PropertyBag";
            ContainerTypeName = containerType.ToCSharpName();
            m_ContainerTypeHasPrivateAccessibility = containerType.HasPrivateAccessibility();
            ContainingTypesArePartial = null != containerType.ContainingType && containerType.ContainingType.IsAccessibleThroughPartial();
        }

        /// <summary>
        /// Gets the sanitized property bag name for the given container type. This generates a unique name based on:
        ///     * Namespace.
        ///     * Containing Type.
        ///     * Generic Type Arguments.
        /// </summary>
        /// <param name="typeSymbol">The <see cref="ITypeSymbol"/> to generate a property bag name for.</param>
        /// <returns>A sanitized property bag name for the given container type.</returns>
        static string GetGeneratedPropertyBagNameOutputFilename(ITypeSymbol typeSymbol)
        {
            if (!LogOptions.LogType.HasFlag(LogType.PropertyBags))
                return string.Empty;

            var builder = new StringBuilder();
            if (null != typeSymbol.ContainingNamespace && !typeSymbol.ContainingNamespace.IsGlobalNamespace)
            {
                builder.Append(typeSymbol.ContainingNamespace.ToString().Replace(".", "_"));
                builder.Append("_");
            }

            foreach (var containingType in typeSymbol.GetContainingTypes())
            {
                builder.Append(containingType.Name);
                builder.Append("_");

                if (containingType is INamedTypeSymbol containingTypeNamedTypeSymbol)
                {
                    foreach (var typeArgument in containingTypeNamedTypeSymbol.TypeArguments)
                    {
                        builder.Append("_");
                        builder.Append(typeArgument.Name);
                    }
                }
            }

            builder.Append(typeSymbol.Name);

            // Special handling for value tuple types.
            if (typeSymbol.IsTupleType)
            {
                foreach (var member in typeSymbol.GetMembers().OfType<IFieldSymbol>())
                {
                    // Skip the named field symbol
                    if (!SymbolEqualityComparer.Default.Equals(member.CorrespondingTupleField, member))
                        continue;

                    builder.Append("_");
                    if (null != member.Type.ContainingNamespace && !member.Type.ContainingNamespace.IsGlobalNamespace)
                    {
                        builder.Append(member.Type.ContainingNamespace.ToString().Replace(".", "_"));
                        builder.Append("_");
                    }

                    builder.Append(member.Type.Name);
                }
            }
            else if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
            {
                foreach (var typeArgument in namedTypeSymbol.TypeArguments)
                {
                    builder.Append("_");
                    if (null != typeArgument.ContainingNamespace && !typeArgument.ContainingNamespace.IsGlobalNamespace)
                    {
                        builder.Append(typeArgument.ContainingNamespace.ToString().Replace(".", "_"));
                        builder.Append("_");
                    }

                    builder.Append(typeArgument.Name);
                }
            }

            builder.Append("_PropertyBag");
            return builder.ToString();
        }

        static HashSet<string> s_MemberTypes = new HashSet<string>();

        /// <summary>
        /// Constructs and returns a set of <see cref="PropertyMemberDefinition"/> for each exposed property of the given <see cref="ITypeSymbol"/>.
        /// </summary>
        /// <param name="typeSymbol">The type symbol to generate properties for.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="PropertyMemberDefinition"/> for all exposed properties.</returns>
        IEnumerable<PropertyMemberDefinition> CreatePropertyMembers(ITypeSymbol typeSymbol)
        {
            var current = typeSymbol;

            // Special handling for named value tuple types.
            if (typeSymbol.IsTupleType)
            {
                var members = typeSymbol.GetMembers().OfType<IFieldSymbol>().ToArray();
                for (int i = 0, itemIndex = 1; i < members.Length; ++i)
                {
                    var member = members[i];
                    if (!SymbolEqualityComparer.Default.Equals(member.CorrespondingTupleField, member))
                        continue;

                    yield return new PropertyMemberDefinition(member, $"Item{itemIndex}");
                    ++itemIndex;
                }

                yield break;
            }

            // Special handling for System.Tuple<> types. This will handle all generic variants.
            if (typeSymbol is INamedTypeSymbol named && named.Name == "Tuple")
            {
                foreach (var member in typeSymbol.GetMembers().OfType<IPropertySymbol>())
                {
                    if (member.Name == "System.Runtime.CompilerServices.ITuple.Item" ||
                        member.Name == "System.Runtime.CompilerServices.ITuple.Length")
                        continue;
                    yield return new PropertyMemberDefinition(member);
                }

                yield break;
            }

            s_MemberTypes.Clear();

            while (null != current && current.Name != "Object")
            {
                foreach (var member in current.GetMembers())
                {
                    if (member is IFieldSymbol field)
                    {
                        if (s_MemberTypes.Contains(member.Name))
                        {
                            continue;
                        }

                        s_MemberTypes.Add(member.Name);

                        if (field.IsStatic || field.IsConst)
                            continue;

                        // Special handling for tuple types.
                        if (field.CorrespondingTupleField != null && !SymbolEqualityComparer.Default.Equals(field.CorrespondingTupleField, field))
                            continue;

                        if (field.Type.TypeKind == TypeKind.Pointer)
                            continue;

                        if (field.HasAttribute("DontCreatePropertyAttribute")) continue;
                        if (!(field.DeclaredAccessibility == Accessibility.Public ||
                              field.HasAttribute("CreatePropertyAttribute") ||
                              field.HasAttribute("SerializeField"))) continue;

                        yield return new PropertyMemberDefinition(member);
                    }

                    if (member is IPropertySymbol property)
                    {
                        if (s_MemberTypes.Contains(member.Name))
                        {
                            continue;
                        }

                        s_MemberTypes.Add(member.Name);

                        if (property.IsStatic)
                            continue;

                        if (property.Type.TypeKind == TypeKind.Pointer)
                            continue;

                        if (property.HasAttribute("DontCreatePropertyAttribute")) continue;
                        if (!property.HasAttribute("CreatePropertyAttribute")) continue;

                        yield return new PropertyMemberDefinition(member);
                    }
                }

                current = current.BaseType;
            }
        }

        void ReportDiagnostic(DiagnosticDescriptor diagnostic)
        {
            if (null == m_DiagnosticDescriptors)
                m_DiagnosticDescriptors = new List<DiagnosticDescriptor>();

            m_DiagnosticDescriptors.Add(diagnostic);
        }

        /// <summary>
        /// Returns all diagnostics reported by the property bag construction.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DiagnosticDescriptor> GetDiagnostics()
        {
            return m_DiagnosticDescriptors ?? Enumerable.Empty<DiagnosticDescriptor>();
        }
    }
}
