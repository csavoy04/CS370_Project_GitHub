using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Unity.Properties.SourceGenerator
{
    static class SymbolExtensions
    {
        /// <summary>
        /// Returns <see langword="true"/> if the symbol has any attribute with the given name.
        /// </summary>
        /// <param name="symbol">The symbol to check.</param>
        /// <param name="attributeName">The attribute name to look for.</param>
        /// <returns><see langword="true"/> if the symbol has any attribute with the given name; <see langword="false"/> otherwise.</returns>
        public static bool HasAttribute(this ISymbol symbol, string attributeName)
            => symbol.GetAttributes().Any(a => a.AttributeClass.Name == attributeName);

        /// <summary>
        /// Enumerates all <see cref="ITypeSymbol"/> for a given <see cref="INamespaceSymbol"/> recursively.
        /// </summary>
        /// <remarks>
        /// This method will skip types contained within the <paramref name="visited"/> set. Any returned types are added to the <paramref name="visited"/> set.
        /// </remarks>
        /// <param name="symbol">The namespace to gather types from.</param>
        /// <param name="visited">A set of types which have already been visited.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> over all types in the given namespace.</returns>
        public static IEnumerable<ITypeSymbol> GetTypeMembersRecurse(this INamespaceSymbol symbol,
            HashSet<ISymbol> visited)
        {
            if (!visited.Add(symbol)) yield break;

            foreach (var member in symbol.GetNamespaceMembers().SelectMany(s => GetTypeMembersRecurse(s, visited)))
                yield return member;

            foreach (var member in symbol.GetTypeMembers().SelectMany(s => GetTypeMembersRecurse(s, visited)))
                yield return member;
        }

        /// <summary>
        /// Enumerates all nested <see cref="ITypeSymbol"/> for a given <see cref="ITypeSymbol"/>. This method will return the given <paramref name="symbol"/>.
        /// </summary>
        /// <remarks>
        /// This method will skip types contained within the <paramref name="visited"/> set. Any returned types are added to the <paramref name="visited"/> set.
        /// </remarks>
        /// <param name="symbol">The symbol to gather types from. This symbol is also returned in the enumerator.</param>
        /// <param name="visited">A set of types which have already been visited.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> over all types in the given type. Including the given type.</returns>
        static IEnumerable<ITypeSymbol> GetTypeMembersRecurse(this ITypeSymbol symbol, HashSet<ISymbol> visited)
        {
            if (!visited.Add(symbol)) yield break;

            yield return symbol;

            foreach (var member in symbol.GetTypeMembers().SelectMany(s => GetTypeMembersRecurse(s, visited)))
                yield return member;
        }

        /// <summary>
        /// Gets the formatted symbol name. This can be used as csharp output.
        /// </summary>
        /// <param name="symbol">The symbol to get the name for.</param>
        /// <returns>The valid C# name for the given type.</returns>
        public static string ToCSharpName(this ITypeSymbol symbol)
            => symbol.ToString().Replace("*", "");

        /// <summary>
        /// Gets the location of the syntax for a specified symbol.
        /// </summary>
        /// <param name="symbol">The symbol to get the location for.</param>
        /// <returns>The syntax location.</returns>
        public static Location GetSyntaxLocation(this ISymbol symbol)
            => symbol.DeclaringSyntaxReferences.First().SyntaxTree
                .GetLocation(symbol.DeclaringSyntaxReferences.First().Span);

        public static List<ITypeSymbol> GetContainingTypes(this ISymbol symbol)
        {
            var result = new List<ITypeSymbol>();

            var current = symbol.ContainingType;

            while (null != current)
            {
                result.Add(current);
                current = current.ContainingType;
            }

            result.Reverse();
            return result;
        }

        /// <summary>
        /// Gets the global name for a type (i.e. global::MyNamespace.MyType<global::MyNamespace.OtherType>)
        /// </summary>
        /// <param name="symbol">The type.</param>
        /// <returns>The global name of the type.</returns>
        public static string GetGlobalSanitizedName(this ITypeSymbol symbol)
        {
            if (symbol.IsTupleType)
            {
                // For named value tuples, we need to lose the names, since they all resolve the same type at runtime.
                var parts = symbol.ToDisplayParts(SymbolDisplayFormat.FullyQualifiedFormat)
                    .Where(d => d.Kind != SymbolDisplayPartKind.FieldName).ToArray();
                return string.Join("", parts);
            }

            return symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        }

        /// <summary>
        /// Indicates whether or not a type is partial.
        /// </summary>
        /// <param name="symbol">The type to query.</param>
        /// <returns><see langword="true"/> if the type is partial; <see langword="false"/> otherwise.</returns>
        public static bool IsPartial(this ITypeSymbol symbol)
        {
            foreach (var reference in symbol.DeclaringSyntaxReferences)
            {
                if (!(reference.GetSyntax() is TypeDeclarationSyntax typeDeclarationSyntax))
                    continue;

                if (typeDeclarationSyntax.HasPartialModifier())
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the top-level base type of the provided type symbol.
        /// </summary>
        /// <param name="symbol">The type symbol to query.</param>
        /// <returns>The top-level base class.</returns>
        public static ITypeSymbol GetRootType(this ITypeSymbol symbol)
        {
            var root = symbol;
            while (root.ContainingType != null)
            {
                root = root.ContainingType;
            }

            return root;
        }

        /// <summary>
        /// Indicate whether a type or any of its containing types are declared as private.
        /// </summary>
        /// <param name="symbol">The type.</param>
        /// <returns><see langword="true"/> if the provided type or any of its containing types are private; <see langword="false"/> otherwise.</returns>
        public static bool HasPrivateAccessibility(this ITypeSymbol symbol)
        {
            while (null != symbol)
            {
                if (symbol.DeclaredAccessibility == Accessibility.Private)
                    return true;
                
                // If this is a nested type and it's not public or internal
                if (null != symbol.ContainingType && !(symbol.DeclaredAccessibility == Accessibility.NotApplicable ||
                                                       symbol.DeclaredAccessibility == Accessibility.Public ||
                                                       symbol.DeclaredAccessibility == Accessibility.Internal))
                {
                    
                    return true;
                }

                symbol = symbol.ContainingType;
            }

            return false;
        }

        /// <summary>
        /// Indicates whether or not a type and all its containing types are declared as partial.
        /// </summary>
        /// <param name="symbol">The type.</param>
        /// <returns><see langword="true"/> if the type and its containing types are declared as partial; <see langword="false"/> otherwise.</returns>
        public static bool IsAccessibleThroughPartial(this ITypeSymbol symbol)
        {
            while (null != symbol)
            {
                if (!symbol.IsPartial())
                    return false;

                symbol = symbol.ContainingType;
            }

            return true;
        }
    }
}
