using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGen.SourceGenerators;

/*
Source Generator incremental que detecta modelos marcados con
[AutoViewModel] y genera automáticamente un ViewModel base
siguiendo una convención (Page{Model}VM).

Este generator:
- NO usa reflection
- NO depende de WPF
- NO se ejecuta en runtime
- Se ejecuta durante la compilación
*/
[Generator]
public sealed class AutoViewModelGenerator : IIncrementalGenerator
{
    /*
    Punto de entrada del generator.
    Aquí se define el pipeline incremental:
    qué nodos observar, cómo filtrarlos y
    qué hacer cuando cambian.
    */
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        /*
        Paso 1:
        Crear un proveedor incremental que observe el árbol de sintaxis.

        Solo nos interesan:
        - Clases (ClassDeclarationSyntax)
        - Que tengan al menos un atributo

        Esto es rápido y evita trabajo innecesario.
        */
        var classesWithAttributes = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, _) =>
                    node is ClassDeclarationSyntax cds &&
                    cds.AttributeLists.Count > 0,
                /*
                Paso 2:
                Transformar el nodo de sintaxis en un símbolo semántico.

                Aquí pasamos de "texto del código"
                a "modelo semántico del compilador".
                */
                transform: static (ctx, _) =>
                {
                    var classDecl = (ClassDeclarationSyntax)ctx.Node;
                    return ctx.SemanticModel.GetDeclaredSymbol(classDecl);
                })
            /*
            Paso 3:
            Filtrar solo símbolos de tipo clase real
            (INamedTypeSymbol).

            Roslyn puede devolver otros símbolos,
            por eso filtramos explícitamente.
            */
            .Where(symbol => symbol is INamedTypeSymbol)
            /*
            Paso 4:
            Convertir el símbolo genérico en INamedTypeSymbol
            para poder usarlo con seguridad.
            */
            .Select((symbol, _) => (INamedTypeSymbol)symbol!);

        /*
        Paso 5:
        Registrar la acción final:
        por cada símbolo válido encontrado,
        se ejecutará el método Generate.
        */
        context.RegisterSourceOutput(
            classesWithAttributes,
            static (spc, symbol) => Generate(spc, symbol)
        );
    }

    /*
    Método que genera el código fuente del ViewModel.
    Se ejecuta una vez por cada clase marcada con [AutoViewModel].
    */
    private static void Generate(
        SourceProductionContext context,
        INamedTypeSymbol model)
    {
        /*
        Paso 6:
        Buscar el atributo [AutoViewModel] en la clase.
        Si no existe, no generamos nada.
        */
        var attribute = model.GetAttributes()
            .FirstOrDefault(a =>
                a.AttributeClass?.Name == "AutoViewModelAttribute");

        if (attribute == null)
            return;

        /*
        Paso 7:
        Leer parámetros opcionales del atributo
        (Prefix, Suffix).
        */
        var prefix = attribute.NamedArguments
            .FirstOrDefault(a => a.Key == "Prefix")
            .Value.Value?.ToString() ?? "Page";

        var suffix = attribute.NamedArguments
            .FirstOrDefault(a => a.Key == "Suffix")
            .Value.Value?.ToString() ?? "VM";

        /*
        Paso 8:
        Construir nombres finales.
        */
        var vmName = $"{prefix}{model.Name}{suffix}";
        var modelNamespace = model.ContainingNamespace.ToDisplayString();

        /*
        ⚠️ Nota:
        Este namespace luego se puede hacer configurable
        desde èl.csproj.
        */
        var vmNamespace = "Cliente.ViewModel.Model";

        /*
        Paso 9:
        Generar el código fuente como texto.
        Este código NO se escribe a disco,
        se inyecta directamente en la compilación.
        */
        var source = $$"""
                       using {{modelNamespace}};

                       namespace {{vmNamespace}};

                       public partial class {{vmName}}
                           : ViewModelServiceBase<{{model.Name}}>
                       {
                       }
                       """;

        /*
        Paso 10:
        Registrar el código generado en el compilador.
        El nombre del archivo es lógico, no físico.
        */
        context.AddSource($"{vmName}.g.cs", source);
    }
}