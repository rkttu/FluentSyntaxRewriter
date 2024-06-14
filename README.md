# FluentSyntaxRewriter

[![NuGet Version](https://img.shields.io/nuget/v/FluentSyntaxRewriter)](https://www.nuget.org/packages/FluentSyntaxRewriter/) ![Build Status](https://github.com/rkttu/FluentSyntaxRewriter/actions/workflows/dotnet.yml/badge.svg) [![GitHub Sponsors](https://img.shields.io/github/sponsors/rkttu)](https://github.com/sponsors/rkttu/)

.NET-based ADMX/ADML parser library and programmatic Windows policy setting/management framework

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

## License

This library follows Apache-2.0 license. See [LICENSE](./LICENSE) file for more information.
