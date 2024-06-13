using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentSyntaxRewriter
{
    /// <summary>
    /// Extension methods for <see cref="SyntaxNode"/> and <see cref="SyntaxTree"/> related to C# syntax.
    /// </summary>
    public static class CSharpSyntaxTreeExtension
    {
        /// <summary>
        /// Adds a new statement to the block.
        /// </summary>
        /// <param name="block">
        /// The block to add the statement to.
        /// </param>
        /// <param name="statements">
        /// The statements to add to the block.
        /// </param>
        /// <returns>
        /// The block with the new statements added.
        /// </returns>
        public static BlockSyntax AddStatements(this BlockSyntax block, params string[] statements)
        {
            if (statements == null || statements.Length < 1)
                return block;

            foreach (var eachStatement in statements)
            {
                var parsedStatement = SyntaxFactory.ParseStatement(string.Concat(eachStatement, Environment.NewLine));
                block = block.WithStatements(block.Statements.Add(parsedStatement));
            }

            return block;
        }

        /// <summary>
        /// Adds a new statement to the block.
        /// </summary>
        /// <param name="methodNode">
        /// The method to add the statement to.
        /// </param>
        /// <param name="statements">
        /// The statements to add to the method.
        /// </param>
        /// <returns>
        /// The method with the new statements added.
        /// </returns>
        public static SyntaxNode AddStatements(this MethodDeclarationSyntax methodNode, params string[] statements)
        {
            var body = methodNode.Body;
            if (body != null)
            {
                var newBody = AddStatements(body, statements);
                return methodNode.WithBody(newBody);
            }
            return methodNode;
        }

        /// <summary>
        /// Adds a new statement to the block.
        /// </summary>
        /// <param name="propertyNode">
        /// The property to add the statement to.
        /// </param>
        /// <param name="statements">
        /// The statements to add to the property.
        /// </param>
        /// <returns>
        /// The property with the new statements added.
        /// </returns>
        public static SyntaxNode AddStatements(this PropertyDeclarationSyntax propertyNode, params string[] statements)
        {
            var accessors = propertyNode.AccessorList.Accessors;
            if (accessors != null)
            {
                var newAccessors = new SyntaxList<AccessorDeclarationSyntax>(accessors.Cast<AccessorDeclarationSyntax>().Select(accessor =>
                {
                    if (accessor.Body != null)
                    {
                        var newBody = AddStatements(accessor.Body, statements);
                        return accessor.WithBody(newBody);
                    }
                    return accessor;
                }));
                return propertyNode.WithAccessorList(SyntaxFactory.AccessorList(newAccessors));
            }
            return propertyNode;
        }

        /// <summary>
        /// Adds a new statement to the block.
        /// </summary>
        /// <param name="eventNode">
        /// The event to add the statement to.
        /// </param>
        /// <param name="statements">
        /// The statements to add to the event.
        /// </param>
        /// <returns>
        /// The event with the new statements added.
        /// </returns>
        public static SyntaxNode AddStatements(this EventDeclarationSyntax eventNode, params string[] statements)
        {
            var accessors = eventNode.AccessorList.Accessors;
            if (accessors != null)
            {
                var newAccessors = new SyntaxList<AccessorDeclarationSyntax>(accessors.Cast<AccessorDeclarationSyntax>().Select(accessor =>
                {
                    if (accessor.Body != null)
                    {
                        var newBody = AddStatements(accessor.Body, statements);
                        return accessor.WithBody(newBody);
                    }
                    return accessor;
                }));
                return eventNode.WithAccessorList(SyntaxFactory.AccessorList(newAccessors));
            }
            return eventNode;
        }

        /// <summary>
        /// Adds a new statement to the block.
        /// </summary>
        /// <typeparam name="TSyntaxNode">
        /// The type of the syntax node.
        /// </typeparam>
        /// <param name="node">
        /// The syntax node to add the statement to.
        /// </param>
        /// <returns>
        /// The syntax node with the new statements added.
        /// </returns>
        public static string GetContainingTypeName<TSyntaxNode>(this TSyntaxNode node)
            where TSyntaxNode : SyntaxNode
        {
            var typeNames = new List<string>();
            var current = (SyntaxNode)node;

            while (current != null)
            {
                if (current is TypeDeclarationSyntax typeNode)
                    typeNames.Insert(0, typeNode.Identifier.Text);

                current = current.Parent;
            }

            if (typeNames.Count > 0)
                return string.Join(".", typeNames);

            return null;
        }

        /// <summary>
        /// Gets the containing namespace of the syntax node.
        /// </summary>
        /// <typeparam name="TSyntaxNode">
        /// The type of the syntax node.
        /// </typeparam>
        /// <param name="node">
        /// The syntax node to get the containing namespace of.
        /// </param>
        /// <returns>
        /// The containing namespace of the syntax node.
        /// </returns>
        public static string GetContainingNamespace<TSyntaxNode>(this TSyntaxNode node)
            where TSyntaxNode : SyntaxNode
        {
            var current = (SyntaxNode)node;

            while (current != null && !(current is NamespaceDeclarationSyntax))
                current = current.Parent;

            if (current is NamespaceDeclarationSyntax namespaceNode)
                return namespaceNode.Name.ToString();

            return string.Empty;
        }

        /// <summary>
        /// Gets the member name of the syntax node.
        /// </summary>
        /// <typeparam name="TSyntaxNode">
        /// The type of the syntax node.
        /// </typeparam>
        /// <param name="node">
        /// The syntax node to get the member name of.
        /// </param>
        /// <returns>
        /// The member name of the syntax node.
        /// </returns>
        public static string GetMemberName<TSyntaxNode>(this TSyntaxNode node)
            where TSyntaxNode : SyntaxNode
        {
            switch (node)
            {
                case MethodDeclarationSyntax methodNode:
                    return methodNode.Identifier.Text;
                case PropertyDeclarationSyntax propertyNode:
                    return propertyNode.Identifier.Text;
                case EventDeclarationSyntax eventNode:
                    return eventNode.Identifier.Text;
                case FieldDeclarationSyntax fieldNode:
                    var variable = fieldNode.Declaration.Variables.FirstOrDefault();
                    return variable.Identifier.Text;
                case VariableDeclaratorSyntax variableNode:
                    return variableNode.Identifier.Text;
                case TypeDeclarationSyntax typeNode:
                    return typeNode.Identifier.Text;
                case NamespaceDeclarationSyntax namespaceNode:
                    return namespaceNode.Name.ToString();
                default:
                    return null;
            }
        }

        /// <summary>
        /// Rename the member of the syntax node.
        /// </summary>
        /// <param name="methodNode">
        /// The method node to rename.
        /// </param>
        /// <param name="nameReplacer">
        /// The function to replace the name of the method.
        /// </param>
        /// <returns>
        /// The method node with the new name.
        /// </returns>
        public static MethodDeclarationSyntax RenameMember(this MethodDeclarationSyntax methodNode, Func<string, string> nameReplacer)
        {
            var currentName = methodNode.Identifier.Text;
            var newName = nameReplacer.Invoke(currentName);

            if (string.IsNullOrWhiteSpace(newName) || string.Equals(currentName, newName, StringComparison.Ordinal))
                return methodNode;

            return methodNode.WithIdentifier(SyntaxFactory.Identifier(newName));
        }

        /// <summary>
        /// Rename the member of the syntax node.
        /// </summary>
        /// <param name="propertyNode">
        /// The property node to rename.
        /// </param>
        /// <param name="nameReplacer">
        /// The function to replace the name of the property.
        /// </param>
        /// <returns>
        /// The property node with the new name.
        /// </returns>
        public static PropertyDeclarationSyntax RenameMember(this PropertyDeclarationSyntax propertyNode, Func<string, string> nameReplacer)
        {
            var currentName = propertyNode.Identifier.Text;
            var newName = nameReplacer.Invoke(currentName);

            if (string.IsNullOrWhiteSpace(newName) || string.Equals(currentName, newName, StringComparison.Ordinal))
                return propertyNode;

            return propertyNode.WithIdentifier(SyntaxFactory.Identifier(newName));
        }

        /// <summary>
        /// Rename the member of the syntax node.
        /// </summary>
        /// <param name="eventNode">
        /// The event node to rename.
        /// </param>
        /// <param name="nameReplacer">
        /// The function to replace the name of the event.
        /// </param>
        /// <returns>
        /// The event node with the new name.
        /// </returns>
        public static EventDeclarationSyntax RenameMember(this EventDeclarationSyntax eventNode, Func<string, string> nameReplacer)
        {
            var currentName = eventNode.Identifier.Text;
            var newName = nameReplacer.Invoke(currentName);

            if (string.IsNullOrWhiteSpace(newName) || string.Equals(currentName, newName, StringComparison.Ordinal))
                return eventNode;

            return eventNode.WithIdentifier(SyntaxFactory.Identifier(newName));
        }

        /// <summary>
        /// Rename the member of the syntax node.
        /// </summary>
        /// <param name="fieldNode">
        /// The field node to rename.
        /// </param>
        /// <param name="nameReplacer">
        /// The function to replace the name of the field.
        /// </param>
        /// <returns>
        /// The field node with the new name.
        /// </returns>
        public static FieldDeclarationSyntax RenameMember(this FieldDeclarationSyntax fieldNode, Func<string, string> nameReplacer)
        {
            var variable = fieldNode.Declaration.Variables.FirstOrDefault();
            if (variable == null)
                return fieldNode;

            var currentName = variable.Identifier.Text;
            var newName = nameReplacer.Invoke(currentName);

            if (string.IsNullOrWhiteSpace(newName) || string.Equals(currentName, newName, StringComparison.Ordinal))
                return fieldNode;

            var newVariable = variable.WithIdentifier(SyntaxFactory.Identifier(newName));
            var newDeclaration = fieldNode.Declaration.WithVariables(SyntaxFactory.SingletonSeparatedList(newVariable));
            return fieldNode.WithDeclaration(newDeclaration);
        }

        /// <summary>
        /// Rename the member of the syntax node.
        /// </summary>
        /// <param name="typeNode">
        /// The type node to rename.
        /// </param>
        /// <param name="nameReplacer">
        /// The function to replace the name of the type.
        /// </param>
        /// <returns>
        /// The type node with the new name.
        /// </returns>
        public static TypeDeclarationSyntax RenameMember(this TypeDeclarationSyntax typeNode, Func<string, string> nameReplacer)
        {
            var currentName = typeNode.Identifier.Text;
            var newName = nameReplacer.Invoke(currentName);

            if (string.IsNullOrWhiteSpace(newName) || string.Equals(currentName, newName, StringComparison.Ordinal))
                return typeNode;

            return typeNode.WithIdentifier(SyntaxFactory.Identifier(newName));
        }

        /// <summary>
        /// Rename the member of the syntax node.
        /// </summary>
        /// <param name="namespaceNode">
        /// The namespace node to rename.
        /// </param>
        /// <param name="nameReplacer">
        /// The function to replace the name of the namespace.
        /// </param>
        /// <returns>
        /// The namespace node with the new name.
        /// </returns>
        public static NamespaceDeclarationSyntax RenameMember(this NamespaceDeclarationSyntax namespaceNode, Func<string, string> nameReplacer)
        {
            var currentName = namespaceNode.Name.ToString();
            var newName = nameReplacer.Invoke(currentName);

            if (string.IsNullOrWhiteSpace(newName) || string.Equals(currentName, newName, StringComparison.Ordinal))
                return namespaceNode;

            var newNamespaceName = SyntaxFactory.ParseName(newName);
            return namespaceNode.WithName(newNamespaceName);
        }

        /// <summary>
        /// Rewrite the syntax node with the rewriter and get the full string with C# formatting.
        /// </summary>
        /// <param name="rewriter">
        /// The rewriter to rewrite the syntax node with.
        /// </param>
        /// <param name="syntaxNode">
        /// The syntax node to rewrite.
        /// </param>
        /// <param name="useTabs">
        /// Whether to use tabs for indentation.
        /// </param>
        /// <param name="indentationSize">
        /// The size of the indentation.
        /// </param>
        /// <param name="indentStyle">
        /// The style of the indentation.
        /// </param>
        /// <param name="newLine">
        /// The new line string.
        /// </param>
        /// <returns>
        /// The full string of the syntax node with C# formatting.
        /// </returns>
        public static string RewriteCSharp(this FluentCSharpSyntaxRewriter rewriter,
            SyntaxNode syntaxNode,
            bool useTabs = false, int indentationSize = 4,
            FormattingOptions.IndentStyle indentStyle = default,
            string newLine = default)
            => rewriter.Visit(syntaxNode).ToFullStringCSharp(useTabs, indentationSize, indentStyle, newLine);

        /// <summary>
        /// Get full string of the syntax node with C# formatting.
        /// </summary>
        /// <param name="syntaxNode">
        /// The syntax node to get the full string of.
        /// </param>
        /// <param name="useTabs">
        /// Whether to use tabs for indentation.
        /// </param>
        /// <param name="indentationSize">
        /// The size of the indentation.
        /// </param>
        /// <param name="indentStyle">
        /// The style of the indentation.
        /// </param>
        /// <param name="newLine">
        /// The new line string.
        /// </param>
        /// <returns>
        /// The full string of the syntax node with C# formatting.
        /// </returns>
        public static string ToFullStringCSharp(this SyntaxNode syntaxNode,
            bool useTabs = false, int indentationSize = 4,
            FormattingOptions.IndentStyle indentStyle = default,
            string newLine = default)
        {
            using (var workspace = new AdhocWorkspace())
            {
                var options = workspace.Options
                    .WithChangedOption(FormattingOptions.UseTabs, LanguageNames.CSharp, useTabs)
                    .WithChangedOption(FormattingOptions.IndentationSize, LanguageNames.CSharp, indentationSize)
                    .WithChangedOption(FormattingOptions.SmartIndent, LanguageNames.CSharp, indentStyle)
                    .WithChangedOption(FormattingOptions.NewLine, LanguageNames.CSharp, newLine ?? Environment.NewLine);
                var formattedRoot = Formatter.Format(syntaxNode, workspace, options);
                return formattedRoot.ToFullString();
            }
        }
    }
}
