using System.Collections;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;

namespace Cliente.View.Items;

/// <summary>
///     Lógica de interacción para VariantesAdd.xaml
/// </summary>
public partial class VariantesAdd : UserControl
{
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(VariantesAdd));

    public static readonly DependencyProperty ItemSelectProperty =
        DependencyProperty.Register(nameof(ItemSelect), typeof(object), typeof(VariantesAdd));

    public static readonly DependencyProperty EditarVarianteCommandProperty =
        DependencyProperty.Register(nameof(EditarVarianteCommand), typeof(IAsyncRelayCommand), typeof(VariantesAdd));

    public static readonly DependencyProperty EliminarVarianteCommandProperty =
        DependencyProperty.Register(nameof(EliminarVarianteCommand), typeof(IAsyncRelayCommand), typeof(VariantesAdd));

    public static readonly DependencyProperty TypeItemProperty =
        DependencyProperty.Register(nameof(TypeItem), typeof(Type), typeof(VariantesAdd));


    public VariantesAdd()
    {
        InitializeComponent();
        RegistrarComandos();
    }

    public required Type TypeItem
    {
        get => (Type)GetValue(TypeItemProperty);
        set => SetValue(TypeItemProperty, value);
    }

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public object ItemSelect
    {
        get => GetValue(ItemSelectProperty);
        set => SetValue(ItemSelectProperty, value);
    }

    public IAsyncRelayCommand EditarVarianteCommand
    {
        get => (IAsyncRelayCommand)GetValue(EditarVarianteCommandProperty);
        set => SetValue(EditarVarianteCommandProperty, value);
    }

    public IAsyncRelayCommand EliminarVarianteCommand
    {
        get => (IAsyncRelayCommand)GetValue(EliminarVarianteCommandProperty);
        set => SetValue(EliminarVarianteCommandProperty, value);
    }

    private async void AddVariante(object sender, RoutedEventArgs e)
    {
        //var Dialog = new FormularioDinamico(new Variantes())
        //{
        //    AceptarCommand = new AsyncRelayCommand<Variantes>(AgregarVariente),
        //    TextHeader = $"Crear {ComponetesHelp.GetNombreEntidad<Variantes>(Pluralidad.TituloS)}",
        //    DialogOpenIdentifier = DialogService.DialogSub01,
        //    DialogNameIdentifier = DialogService.DialogSub02
        //};

        //await DialogService.Instance.MostrarDialogo(Dialog);
    }
    //public Task AgregarVariente(Variantes? element)
    //{
    //    if (element != null && ItemsSource is ObservableCollection<Variantes> collection)
    //        collection.Add(element);
    //    return Task.CompletedTask;
    //}

    public void RegistrarComandos()
    {
        //EditarVarianteCommand = new AsyncRelayCommand<Variantes>(EditarVarianteAsync);

        //EliminarVarianteCommand = new AsyncRelayCommand<Variantes>(EliminarVarianteAsync);
    }

    //private async Task EditarVarianteAsync(Variantes? atributo)
    //{
    //    if (atributo == null) return;

    //    await DialogService.MostrarFormularioDinamicoAsyncSubDialog01(atributo.Clone(), "Editar Variante", (a) =>
    //    {
    //        atributo.Value = a.Value;
    //        atributo.Descripcion = a.Descripcion;
    //        return Task.CompletedTask;
    //    });

    //}

    //private Task EliminarVarianteAsync(Variantes? atributo)
    //{
    //    if (atributo == null) return Task.CompletedTask;

    //    if (ItemsSource is ObservableCollection<Variantes> collection)
    //        collection.Remove(atributo);

    //    return Task.CompletedTask;
    //}
}