using System.Collections;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Cliente.Converter;
using Utilidades.Attributes;

namespace Cliente.View.Model;

public partial class PageAreasV : UserControl
{
    public PageAreasV()
    {
        InitializeComponent();

        // Esperamos a que el control ya tenga DataContext
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        GenerateColumns();
    }

    private void GenerateColumns()
    {
        // 1️⃣ Obtener el ItemsSource actual del DataGrid
        var itemsSource = DataGridSolicitados.ItemsSource;
        if (itemsSource == null)
            return;

        // 2️⃣ Determinar el tipo real del item (Empleado)
        var enumerableType = itemsSource.GetType();

        if (!enumerableType.IsGenericType)
            return;

        var itemType = enumerableType.GetGenericArguments().FirstOrDefault();
        if (itemType == null)
            return;

        // 3️⃣ Obtener propiedades visibles por atributo Vista
        var properties = itemType.GetProperties()
            .Select(p => new
            {
                Prop = p,
                Vista = p.GetCustomAttribute<VistaAttribute>(),
                Solicitar = p.GetCustomAttribute<SolicitarAttribute>()
            })
            .Where(x => x.Vista != null && x.Vista.Visible)
            .OrderBy(x => x.Vista!.Orden)
            .ToList();

        // 4️⃣ Limpiar columnas actuales
        DataGridSolicitados.Columns.Clear();

        // 5️⃣ Crear columnas dinámicamente
        foreach (var item in properties)
        {
            var prop = item.Prop;
            var vista = item.Vista;
            var solicitar = item.Solicitar;

            var header =
                vista?.Nombre ??
                solicitar?.Nombre ??
                prop.Name;

            var isEnumerable =
                typeof(IEnumerable).IsAssignableFrom(prop.PropertyType)
                && prop.PropertyType != typeof(string);

            var binding = isEnumerable
                ? new Binding($"{prop.Name}.Count")
                : new Binding(prop.Name);

            DataGridSolicitados.Columns.Add(
                new DataGridTextColumn
                {
                    Header = header,
                    Binding = binding
                });
        }
    }

}

