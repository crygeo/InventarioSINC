using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Cliente.Obj.Model;
using Cliente.Services;
using Cliente.src.Services.Model;
using CommunityToolkit.Mvvm.Input;
using Shared.Extensions;

namespace Cliente.View.Items;

/// <summary>
/// Lógica de interacción para IdentificadoresSelect.xaml
/// </summary>
public partial class IdentificadoresSelect : UserControl
{

    public static readonly DependencyProperty IdentificadoresOrigenProperty = DependencyProperty.Register(nameof(IdentificadoresOrigen), typeof(ObservableCollection<Identificador>), typeof(IdentificadoresSelect));
    public static readonly DependencyProperty AgregarCommandProperty = DependencyProperty.Register(nameof(AgregarCommand), typeof(IAsyncRelayCommand), typeof(IdentificadoresSelect));
    public static readonly DependencyProperty EditarCommandProperty = DependencyProperty.Register(nameof(EditarCommand), typeof(IAsyncRelayCommand), typeof(IdentificadoresSelect));
    public static readonly DependencyProperty EliminarCommandProperty = DependencyProperty.Register(nameof(EliminarCommand), typeof(IAsyncRelayCommand), typeof(IdentificadoresSelect));

    public ObservableCollection<Identificador> IdentificadoresOrigen
    {
        get => (ObservableCollection<Identificador>)GetValue(IdentificadoresOrigenProperty);
        set => SetValue(IdentificadoresOrigenProperty, value);
    }
    public IAsyncRelayCommand AgregarCommand
    {
        get => (IAsyncRelayCommand)GetValue(AgregarCommandProperty);
        set => SetValue(AgregarCommandProperty, value);
    }

    public IAsyncRelayCommand EditarCommand
    {
        get => (IAsyncRelayCommand)GetValue(EditarCommandProperty);
        set => SetValue(EditarCommandProperty, value);
    }

    public IAsyncRelayCommand EliminarCommand
    {
        get => (IAsyncRelayCommand)GetValue(EliminarCommandProperty);
        set => SetValue(EliminarCommandProperty, value);
    }

    public IdentificadoresSelect()
    {
        InitializeComponent();
        //agregarcommand = new asyncrelaycommand<identificador>(crearnuevovalor);
        //EditarCommand = new AsyncRelayCommand<Atributo>(EditarValorCommand);
        //EliminarCommand = new AsyncRelayCommand<Atributo>(EliminarValorCommand);

        IdentificadoresOrigen = ServiceFactory.GetService<Identificador>().Collection;

    }

    //private async Task CrearNuevoValor(Identificador? identificador)
    //{
    //    if (identificador != null)
    //        await DialogService.MostrarFormularioDinamicoAsyncSubDialog01<Atributo>(new Atributo(), $"Agregar nuevo \"{identificador.Name}\"", (a) => AgregarValor(a, identificador));
    //}

    //private async Task AgregarValor(Atributo valor, Identificador identificador)
    //{
    //    var service = ServiceFactory.GetService<Identificador>();

    //    var cloneId = identificador.Clone();

    //    if(cloneId.Valores == null)
    //        cloneId.Valores = new ObservableCollection<Atributo>();

    //    if(cloneId.Valores is not ObservableCollection<Atributo> coll)
    //        return;

    //    coll.Add(valor);
    //    var result = await service.UpdateAsync(identificador.Id, cloneId);

    //    await DialogService.Instance.ValidarRespuesta(result, "Valor agregado con exito.");


    //}

    //public async Task EditarValorCommand(Atributo? atributo)
    //{

    //}

    //public async Task EliminarValorCommand(Atributo? atributo)
    //{

    //}


}