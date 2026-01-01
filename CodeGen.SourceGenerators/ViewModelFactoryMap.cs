using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.SourceGenerators;

/// <summary>
/// Source Generator que construye automáticamente el
/// diccionario Modelo → ViewModel dentro de ViewModelFactory.
/// </summary>
[Generator]
public sealed class ViewModelFactoryMap : IIncrementalGenerator
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
    
    private static void GenerateFactory(
        SourceProductionContext context,
        ImmutableArray<INamedTypeSymbol> models)
    {
        var entries = new List<string>();

        foreach (var model in models)
        {
            if (!model.GetAttributes()
                    .Any(a => a.AttributeClass?.Name == "AutoViewModelAttribute"))
                continue;

            var modelName = model.Name;
            var vmName = $"Page{modelName}VM";

            entries.Add(
                $"{{ typeof({modelName}), () => new {vmName}() }}");
        }

        if (entries.Count == 0)
            return;

        var source = $$"""
                       using System;
                       using System.Collections.Generic;
                       using Cliente.Obj.Model;

                       namespace Cliente.ViewModel.Model;

                       public static partial class ViewModelFactory
                       {
                           private static readonly Dictionary<Type, Func<object>> _map =
                               new()
                               {
                                   {{string.Join(",\n                ", entries)}}
                               };
                       }
                       """;

        context.AddSource("ViewModelFactory.Map.g.cs", source);
    }


}