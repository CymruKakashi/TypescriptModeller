using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TypescriptModeller
{
    public class ModelParser: CSharpSyntaxWalker
    {
        public readonly Dictionary<string, List<AttributeInfo>> models = new Dictionary<string, List<AttributeInfo>>();
        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            if(node?.Parent == null)
                return;

            var interfaceNode = node.Parent as InterfaceDeclarationSyntax;
            if(interfaceNode == null)
                return;

            if (!models.ContainsKey(interfaceNode.Identifier.ValueText))
                models.Add(interfaceNode.Identifier.ValueText, new List<AttributeInfo>());

            AttributeInfo info = new AttributeInfo{
                name = node.Identifier.ValueText.Substring(0,1).ToLower() + node.Identifier.ValueText.Substring(1), // format the name thisCase
                type = (node.Type as IdentifierNameSyntax)?.Identifier.ValueText // Type like Guid
                        ?? (node.Type as PredefinedTypeSyntax)?.Keyword.ValueText // keyword type like string
                        ?? (((node.Type as NullableTypeSyntax)?.ElementType as IdentifierNameSyntax)?.Identifier.ValueText) // Nullable like Guid?
                        ?? (((node.Type as NullableTypeSyntax)?.ElementType as PredefinedTypeSyntax)?.Keyword.ValueText) // Nullable like int?
            };
            //add the question mark to indicate that it's nullable
            if((node.Type as NullableTypeSyntax) != null)
                info.type = info.type + "?";

            models[interfaceNode.Identifier.ValueText].Add(info);
        }

        public CompilationUnitSyntax GetRoot(string code)
        {
            if(code == null)
                return null;
            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            CompilationUnitSyntax root = (CompilationUnitSyntax)tree.GetRoot();
            return root;
        }

    }
}