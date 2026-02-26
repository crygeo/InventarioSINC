using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.SourceGenerators;

[Generator]
public class ServiceRegistrationGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var services = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (n, _) =>
                    n is ClassDeclarationSyntax cds &&
                    cds.AttributeLists.Count > 0,
                transform: static (ctx, _) =>
                    ctx.SemanticModel.GetDeclaredSymbol(ctx.Node))
            .Where(s => s is INamedTypeSymbol)
            .Select((s, _) => (INamedTypeSymbol)s!);

        context.RegisterSourceOutput(
            services.Collect(),
            static (spc, list) => GenerateRegistration(spc, list)
        );
    }

    private static void GenerateRegistration(
        SourceProductionContext spc,
        ImmutableArray<INamedTypeSymbol> list)
    {
        var entries = new List<string>();

        foreach (var service in list)
        {
            var attr = service.GetAttributes()
                .FirstOrDefault(a => a.AttributeClass?.Name == "AutoServiceAttribute");

            if (attr is null)
                continue;

            var fullName = service.ToDisplayString();

            entries.Add($$"""
                services.AddScoped<{{fullName}}>();
                """);
        }

        if (entries.Count == 0)
            return;

        var source = $$"""
            using Microsoft.Extensions.DependencyInjection;
            
            namespace Servidor.Helper;

            public static partial class AutoServiceRegistration
            {
                public static void AddAutoServices(this IServiceCollection services)
                {
                    {{string.Join("\n        ", entries)}}
                }
            }
            """;

        spc.AddSource("AutoServiceRegistration.g.cs", source);
    }
}