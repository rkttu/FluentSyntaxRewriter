using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentSyntaxRewriter.Test
{
    public class FluentSyntaxRewriterTest
    {
        [Fact]
        public void ChangeMethodName_AddStatements()
        {
            // arrange
            var parsedCode = CSharpSyntaxTree.ParseText("""
namespace MyNamespace {
	using System;
	
	public struct MyExtension {
		public static int Hello() {
		}
		public static int Say() { return 1; }
	}
}
""").GetRoot();

            // act
            var modifiedCode = FluentCSharpSyntaxRewriter.Define()
                .WithVisitMethodDeclaration((_, d) =>
                {
                    if (d.GetContainingNamespace() != "MyNamespace")
                        return d;
                    if (d.GetContainingTypeName() != "MyExtension")
                        return d;
                    if (d.GetMemberName() != "Hello")
                        return d;
                    return d
                        .RenameMember(s => $"Modified_{s}")
                        .AddStatements("return 123;");
                })
                .RewriteCSharp(parsedCode);

            // assert
            Assert.Contains("Modified_Hello", modifiedCode, StringComparison.Ordinal);
            Assert.Contains("return 123;", modifiedCode, StringComparison.Ordinal);
        }

        [Fact]
        public void ChangeIdentifierTokens()
        {
            var parsedCode = CSharpSyntaxTree.ParseText("""
/// <summary>
/// Indicates that a specific value entry should be deleted from the registry.
/// </summary>
public sealed class __TypeName__ {
	private static readonly Lazy<__TypeName__> _instanceOf = new Lazy<__TypeName__>(
		() => new __TypeName__(),
		LazyThreadSafetyMode.None);
	
	public static __TypeName__ Instance => _instanceOf.Value;
	
	internal __TypeName__() :
		base()
	{ }
}
""").GetRoot();

            var modifiedCode = FluentCSharpSyntaxRewriter
                .Define()
                .WithVisitToken((_, token) =>
                {
                    if (token.IsKind(SyntaxKind.IdentifierToken) &&
                        string.Equals(token.ValueText, "__TypeName__", StringComparison.Ordinal))
                        return SyntaxFactory.Identifier("AType").WithTriviaFrom(token);
                    return token;
                })
                .Visit(parsedCode)
                .ToFullStringCSharp();

            Assert.DoesNotContain("__TypeName__", modifiedCode, StringComparison.Ordinal);
        }

        [Fact]
        public void CombineCodes_RenameMember_ManipulateUsings()
        {
            var classCode = SyntaxFactory.ParseMemberDeclaration("""
/// <summary>
/// </summary>
public sealed class __TypeName__ {
	private static readonly Lazy<__TypeName__> _instanceOf = new Lazy<__TypeName__>(
		() => new __TypeName__(),
		LazyThreadSafetyMode.None);
	
	public static __TypeName__ Instance => _instanceOf.Value;
	
	internal __TypeName__() :
		base()
	{ }
}
""");

            var modifiedClassCode = FluentCSharpSyntaxRewriter
                .Define()
                .WithVisitToken((_, token) =>
                {
                    if (token.IsKind(SyntaxKind.IdentifierToken) &&
                        string.Equals(token.ValueText, "__TypeName__", StringComparison.Ordinal))
                        return SyntaxFactory.Identifier("AType").WithTriviaFrom(token);

                    return token;
                })
                .Visit(classCode)
                .ParseMemberDeclaration();

            var namespaceCode = CSharpSyntaxTree.ParseText("""
namespace __ProjectNamespace__ {
}
""").GetRoot();

            var modifiedNamespaceCode = FluentCSharpSyntaxRewriter
                .Define()
                .WithVisitNamespaceDeclaration((_, ns) =>
                {
                    ns = ns.AddUsings("System", "System.Collections.Generic").OrderUsings().DistinctUsings().RenameMember(_ => "TheProject");
                    ns = ns.AddMembers(modifiedClassCode);
                    return ns;
                })
                .Visit(namespaceCode)
                .ToFullStringCSharp();

            Assert.Contains("using System;", modifiedNamespaceCode, StringComparison.Ordinal);
            Assert.Contains("using System.Collections.Generic;", modifiedNamespaceCode, StringComparison.Ordinal);
            Assert.Contains("TheProject", modifiedNamespaceCode, StringComparison.Ordinal);
            Assert.Contains("AType", modifiedNamespaceCode, StringComparison.Ordinal);
        }

        [Fact]
        public void AddXmlDocTest()
        {
            var field = SyntaxFactory.ParseMemberDeclaration(
                """
			    static int z = 0;
			    """);

            var code = field.AddXmlDocumentation(summary: "Test")
                .ToFullStringCSharp();

            Assert.Contains($"/// <summary>Test</summary>{Environment.NewLine}", code, StringComparison.Ordinal);
        }

        [Fact]
        public async Task GetCompilationUnitTest()
        {
            var s = CSharpSyntaxTree.ParseText(
                """
                namespace A
                {
                    public class B
                    {
                    }
                }
                """);

            Assert.NotNull(s.GetCompilationUnitRoot());
            Assert.NotNull(await s.GetCompilationUnitSyntaxAsync());
            Assert.True(s.TryGetCompilationUnitSyntax(out var syntax));
            Assert.NotNull(syntax);
        }
    }
}