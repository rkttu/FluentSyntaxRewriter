using Microsoft.CodeAnalysis.CSharp;

namespace FluentSyntaxRewriter.Test
{
    public class FluentSyntaxRewriterTest
    {
        [Fact]
        public void ModificationTest()
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
    }
}