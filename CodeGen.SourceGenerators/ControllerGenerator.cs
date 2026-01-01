using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.SourceGenerators;

[Generator]
public class ControllerGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var models = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (n, _) =>
                    n is ClassDeclarationSyntax cds &&
                    cds.AttributeLists.Count > 0,
                transform: static (ctx, _) =>
                    ctx.SemanticModel.GetDeclaredSymbol(ctx.Node))
            .Where(s => s is INamedTypeSymbol)
            .Select((s, _) => (INamedTypeSymbol)s!);

        context.RegisterSourceOutput(
            models.Collect(),
            static (spc, list) => GenerateFactory(spc, list)
        );
    }

    private static void GenerateFactory(SourceProductionContext spc, ImmutableArray<INamedTypeSymbol> list)
    {
        var entries = new List<string>();

        foreach (var model in list)
        {
            if (!model.GetAttributes()
                    .Any(a => a.AttributeClass?.Name == "AutoControllerAttribute"))
                continue;

            var controllerName = model.Name;
            var controllerFullName = $"{controllerName}Controller";

            entries.Add($$"""
                          [ApiController]
                          [Route("api/[controller]")]
                          public partial class {{controllerFullName}} : BaseController<{{controllerName}}> {}
                          """);

        }

        if (entries.Count == 0)
            return;

        var source = $$"""
                       using Microsoft.AspNetCore.Mvc;
                       using Servidor.Controllers;
                       using Servidor.Model;

                       namespace Servidor.Controllers;
                       
                        {{string.Join("\n", entries)}}
                       """;

        spc.AddSource("Controllers.all.g.cs", source);
    }
}