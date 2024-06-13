using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

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
    }
}