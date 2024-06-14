using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

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

        /// <summary>
        /// Get the identifier of the name syntax.
        /// </summary>
        /// <typeparam name="TNameSyntax">
        /// The type of the name syntax.
        /// </typeparam>
        /// <param name="nameSyntax">
        /// The name syntax to get the identifier of.
        /// </param>
        /// <returns>
        /// The identifier of the name syntax.
        /// </returns>
        public static string GetIdentifier<TNameSyntax>(this TNameSyntax nameSyntax)
            where TNameSyntax : NameSyntax
        {
            switch (nameSyntax)
            {
                case IdentifierNameSyntax identifierName:
                    return identifierName.Identifier.Text;

                case QualifiedNameSyntax qualifiedName:
                    var left = GetIdentifier(qualifiedName.Left);
                    var right = GetIdentifier(qualifiedName.Right);
                    return string.Join(".", new string[] { left, right, });

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Get the identifier of the name syntax.
        /// </summary>
        /// <typeparam name="TBaseNamespaceDeclarationSyntax">
        /// The type of the base namespace declaration syntax.
        /// </typeparam>
        /// <param name="ns">
        /// The namespace declaration syntax to add usings to.
        /// </param>
        /// <param name="namespacesToAdd">
        /// The namespaces to add to the namespace declaration.
        /// </param>
        /// <returns>
        /// The namespace declaration syntax with the new usings added.
        /// </returns>
        public static TBaseNamespaceDeclarationSyntax AddUsings<TBaseNamespaceDeclarationSyntax>(this TBaseNamespaceDeclarationSyntax ns, params string[] namespacesToAdd)
            where TBaseNamespaceDeclarationSyntax : BaseNamespaceDeclarationSyntax
        {
            if (namespacesToAdd == null || namespacesToAdd.Length < 1)
                return ns;

            var list = ns.Usings;
            foreach (var eachNamespace in namespacesToAdd)
                list = list.Add(SyntaxFactory.UsingDirective(SyntaxFactory.ParseTypeName(eachNamespace)));

            return (TBaseNamespaceDeclarationSyntax)ns.WithUsings(list);
        }

        /// <summary>
        /// Get the identifier of the name syntax.
        /// </summary>
        /// <typeparam name="TBaseNamespaceDeclarationSyntax">
        /// The type of the base namespace declaration syntax.
        /// </typeparam>
        /// <param name="ns">
        /// The namespace declaration syntax to remove usings from.
        /// </param>
        /// <param name="namespacesToRemove">
        /// The namespaces to remove from the namespace declaration.
        /// </param>
        /// <returns>
        /// The namespace declaration syntax with the usings removed.
        /// </returns>
        public static TBaseNamespaceDeclarationSyntax RemoveUsings<TBaseNamespaceDeclarationSyntax>(this TBaseNamespaceDeclarationSyntax ns, params string[] namespacesToRemove)
            where TBaseNamespaceDeclarationSyntax : BaseNamespaceDeclarationSyntax
        {
            if (namespacesToRemove == null || namespacesToRemove.Length < 1)
                return ns;

            var newList = new SyntaxList<UsingDirectiveSyntax>();

            foreach (var eachNamespace in ns.Usings)
            {
                var text = eachNamespace.Name.GetIdentifier();

                if (namespacesToRemove.Contains(text, StringComparer.Ordinal))
                    continue;

                newList = newList.Add(SyntaxFactory.UsingDirective(SyntaxFactory.ParseTypeName(text)));
            }

            return (TBaseNamespaceDeclarationSyntax)ns.WithUsings(newList);
        }

        /// <summary>
        /// Get the identifier of the name syntax.
        /// </summary>
        /// <typeparam name="TBaseNamespaceDeclarationSyntax">
        /// The type of the base namespace declaration syntax.
        /// </typeparam>
        /// <param name="ns">
        /// The namespace declaration syntax to distinct usings from.
        /// </param>
        /// <returns>
        /// The namespace declaration syntax with the distinct usings.
        /// </returns>
        public static TBaseNamespaceDeclarationSyntax DistinctUsings<TBaseNamespaceDeclarationSyntax>(this TBaseNamespaceDeclarationSyntax ns)
            where TBaseNamespaceDeclarationSyntax : BaseNamespaceDeclarationSyntax
        {
            return (TBaseNamespaceDeclarationSyntax)ns.WithUsings(new SyntaxList<UsingDirectiveSyntax>(
                Helpers.DistinctBy(ns.Usings, x => x.Name.GetIdentifier(), StringComparer.Ordinal)));
        }

        /// <summary>
        /// Order the usings in ascending order.
        /// </summary>
        /// <typeparam name="TBaseNamespaceDeclarationSyntax">
        /// The type of the base namespace declaration syntax.
        /// </typeparam>
        /// <param name="ns">
        /// The namespace declaration syntax to order usings from.
        /// </param>
        /// <returns>
        /// The namespace declaration syntax with the ordered usings.
        /// </returns>
        public static TBaseNamespaceDeclarationSyntax OrderUsings<TBaseNamespaceDeclarationSyntax>(this TBaseNamespaceDeclarationSyntax ns)
            where TBaseNamespaceDeclarationSyntax : BaseNamespaceDeclarationSyntax
        {
            return (TBaseNamespaceDeclarationSyntax)ns.WithUsings(new SyntaxList<UsingDirectiveSyntax>(
                ns.Usings.OrderBy(x => x.Name.GetIdentifier(), StringComparer.Ordinal)));
        }

        /// <summary>
        /// Order the usings in descending order.
        /// </summary>
        /// <typeparam name="TBaseNamespaceDeclarationSyntax">
        /// The type of the base namespace declaration syntax.
        /// </typeparam>
        /// <param name="ns">
        /// The namespace declaration syntax to order usings from.
        /// </param>
        /// <returns>
        /// The namespace declaration syntax with the ordered usings.
        /// </returns>
        public static TBaseNamespaceDeclarationSyntax OrderUsingsDescending<TBaseNamespaceDeclarationSyntax>(this TBaseNamespaceDeclarationSyntax ns)
            where TBaseNamespaceDeclarationSyntax : BaseNamespaceDeclarationSyntax
        {
            return (TBaseNamespaceDeclarationSyntax)ns.WithUsings(new SyntaxList<UsingDirectiveSyntax>(
                ns.Usings.OrderByDescending(x => x.Name.GetIdentifier(), StringComparer.Ordinal)));
        }

        /// <summary>
        /// Parse a member declaration from the syntax node.
        /// </summary>
        /// <typeparam name="TSyntaxNode">
        /// The type of the syntax node.
        /// </typeparam>
        /// <param name="node">
        /// The syntax node to parse the member declaration from.
        /// </param>
        /// <param name="offset">
        /// The offset to start parsing from.
        /// </param>
        /// <param name="options">
        /// The parse options.
        /// </param>
        /// <param name="consumeFullText">
        /// Whether to consume the full text of the syntax node.
        /// </param>
        /// <returns>
        /// The member declaration parsed from the syntax node.
        /// </returns>
        public static MemberDeclarationSyntax ParseMemberDeclaration<TSyntaxNode>(this TSyntaxNode node, int offset = 0, ParseOptions options = default, bool consumeFullText = true)
            where TSyntaxNode : SyntaxNode
            => SyntaxFactory.ParseMemberDeclaration(node.ToFullString(), offset, options, consumeFullText);

        /// <summary>
        /// Add XML documentation to the member declaration.
        /// </summary>
        /// <typeparam name="TMemberDeclarationSyntax">
        /// The type of the member declaration.
        /// </typeparam>
        /// <param name="member">
        /// The member declaration to add the XML documentation to.
        /// </param>
        /// <param name="summary">
        /// The summary of the XML documentation.
        /// </param>
        /// <param name="remarks">
        /// The remarks of the XML documentation.
        /// </param>
        /// <param name="returns">
        /// The returns of the XML documentation.
        /// </param>
        /// <param name="parameters">
        /// The parameters of the XML documentation.
        /// </param>
        /// <returns>
        /// The member declaration with the XML documentation added.
        /// </returns>
        public static TMemberDeclarationSyntax AddXmlDocumentation<TMemberDeclarationSyntax>(this TMemberDeclarationSyntax member,
            string summary = default, string remarks = default, string returns = default,
            IReadOnlyDictionary<string, string> parameters = default)
            where TMemberDeclarationSyntax : MemberDeclarationSyntax
        {
            var list = new List<XmlNodeSyntax>();

            if (summary != null)
                list.Add(SyntaxFactory.XmlSummaryElement(SyntaxFactory.XmlText(summary)));

            if (remarks != null)
                list.Add(SyntaxFactory.XmlRemarksElement(SyntaxFactory.XmlText(remarks)));

            if (parameters != null && parameters.Count > 0)
            {
                foreach (var eachParameter in parameters)
                    list.Add(SyntaxFactory.XmlParamElement(eachParameter.Key, SyntaxFactory.XmlText(eachParameter.Value)));
            }

            if (returns != null)
                list.Add(SyntaxFactory.XmlReturnsElement(SyntaxFactory.XmlText(returns)));

            if (list.Count < 1)
                return member;

            var trivia = SyntaxFactory.Trivia(SyntaxFactory.DocumentationComment(list.ToArray()));
            var leadingTrivia = member.GetLeadingTrivia().AddRange(new SyntaxTrivia[] { trivia, SyntaxFactory.Whitespace(Environment.NewLine), });
            return member.WithLeadingTrivia(leadingTrivia);
        }

        /// <summary>
        /// Get the compilation unit syntax from the syntax tree.
        /// </summary>
        /// <typeparam name="TSyntaxTree">
        /// The type of the syntax tree.
        /// </typeparam>
        /// <param name="syntaxTree">
        /// The syntax tree to get the compilation unit syntax from.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The compilation unit syntax from the syntax tree.
        /// </returns>
        public static CompilationUnitSyntax GetCompilationUnitSyntax<TSyntaxTree>(this TSyntaxTree syntaxTree,
            CancellationToken cancellationToken = default)
            where TSyntaxTree : SyntaxTree
            => syntaxTree?.GetRoot(cancellationToken) as CompilationUnitSyntax;

        /// <summary>
        /// Get the compilation unit syntax from the syntax tree asynchronously.
        /// </summary>
        /// <typeparam name="TSyntaxTree">
        /// The type of the syntax tree.
        /// </typeparam>
        /// <param name="syntaxTree">
        /// The syntax tree to get the compilation unit syntax from.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The compilation unit syntax from the syntax tree.
        /// </returns>
        public static async Task<CompilationUnitSyntax> GetCompilationUnitSyntaxAsync<TSyntaxTree>(this TSyntaxTree syntaxTree,
            CancellationToken cancellationToken = default)
            where TSyntaxTree : SyntaxTree
            => (syntaxTree != null) ? await syntaxTree.GetRootAsync(cancellationToken).ConfigureAwait(false) as CompilationUnitSyntax : null;

        /// <summary>
        /// Try to get the compilation unit syntax from the syntax tree.
        /// </summary>
        /// <typeparam name="TSyntaxTree">
        /// The type of the syntax tree.
        /// </typeparam>
        /// <param name="syntaxTree">
        /// The syntax tree to get the compilation unit syntax from.
        /// </param>
        /// <param name="compilationUnitSyntax">
        /// The compilation unit syntax from the syntax tree.
        /// </param>
        /// <returns>
        /// Whether the compilation unit syntax was successfully retrieved.
        /// </returns>
        public static bool TryGetCompilationUnitSyntax<TSyntaxTree>(this TSyntaxTree syntaxTree,
            out CompilationUnitSyntax compilationUnitSyntax)
            where TSyntaxTree : SyntaxTree
        {
            if (syntaxTree == null || !syntaxTree.TryGetRoot(out SyntaxNode root) || root == null)
            {
                compilationUnitSyntax = null;
                return false;
            }
            else
            {
                compilationUnitSyntax = root as CompilationUnitSyntax;
                return (compilationUnitSyntax != null);
            }
        }
    }
}
