# FluentSyntaxRewriter

[![NuGet Version](https://img.shields.io/nuget/v/FluentSyntaxRewriter)](https://www.nuget.org/packages/FluentSyntaxRewriter/) ![Build Status](https://github.com/rkttu/FluentSyntaxRewriter/actions/workflows/dotnet.yml/badge.svg) [![GitHub Sponsors](https://img.shields.io/github/sponsors/rkttu)](https://github.com/sponsors/rkttu/)

A library to help you use the Roslyn APIs for analysing and rewriting the .NET programming language in a fluent way.

## Minimum Requirements

- Requires a platform with .NET Standard 2.0 or later, and Windows Vista+, Windows Server 2008+
  - Supported .NET Version: .NET Core 2.0+, .NET 5+, .NET Framework 4.6.1+, Mono 5.4+, UWP 10.0.16299+, Unity 2018.1+

## How to use

### Rename existing member method and add statements

```csharp
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

Console.Out.WriteLine(modifiedCode);
```

### Replace Identifier Token

```csharp
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

Console.Out.WriteLine(modifiedCode);
```

### Add Using References, Rename Member

```csharp
var namespaceCode = CSharpSyntaxTree.ParseText(
	"""
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

Console.Out.WriteLine(modifiedNamespaceCode);
```

### Add XML Documentation to Member

```csharp
var field = SyntaxFactory.ParseMemberDeclaration(
	"""
	static int z = 0;
	""");

var code = field.AddXmlDocumentation(summary: "Test")
.ToFullStringCSharp();

Console.Out.WriteLine(code);
```

### Get Compilation Unit

```csharp
var s = CSharpSyntaxTree.ParseText(
	"""
	namespace A
	{
	    public class B
	    {
	    }
	}
	""");

var unit1 = s.GetCompilationUnitRoot();
var unit2 = await s.GetCompilationUnitSyntaxAsync();
var result = s.TryGetCompilationUnitSyntax(out var unit3);
```

### Chained method calls

```csharp
var typeName = "DeleteValueType";

var template = SyntaxFactory.ParseMemberDeclaration(
	"""
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

var code = FluentCSharpSyntaxRewriter
	.Define()
	.WithVisitToken((_, token) =>
	{
		if (token.IsKind(SyntaxKind.IdentifierToken) &&
			string.Equals(token.ValueText, "__TypeName__", StringComparison.Ordinal))
			return SyntaxFactory.Identifier(typeName).WithTriviaFrom(token);

		return token;
	})
	.WithVisitClassDeclaration((_, token) =>
	{
		token = FluentSyntaxRewriter.CSharpSyntaxTreeExtension.AddXmlDocumentation(token,
			summary: "Indicates that a specific value entry should be deleted from the registry.");
		return token;
	})
	.Visit(template)
	.ToFullStringCSharp();

Console.Out.WriteLine(code);
```

## License

This library follows Apache-2.0 license. See [LICENSE](./LICENSE) file for more information.
