using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Unity.Properties.SourceGenerator
{
    public class OptInCodeGenerationSyntaxReceiver : ISyntaxReceiver
    {
        public readonly List<AttributeSyntax> Attributes = new List<AttributeSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is AttributeSyntax attribute &&
                (null == attribute.ArgumentList || attribute.ArgumentList.Arguments.Count == 0) &&
                attribute.Name.ToString().Contains("GeneratePropertyBagsForAssembly"))
            {
                Attributes.Add(attribute);
            }
        }
    }
}
