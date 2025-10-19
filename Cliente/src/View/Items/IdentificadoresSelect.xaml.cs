using Cliente.Obj.Model;
using Cliente.Services;
using Cliente.Services.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.Extensions;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Cliente.View.Items;

/// <summary>
/// Componente auxiliar reutilizable tipo UserControl para seleccionar un único 
/// ElementoJerarquico por cada Identificador disponible.
/// 
/// Se comporta como un selector de múltiples ComboBox en línea horizontal, donde cada ComboBox 
/// muestra una lista de valores asociados a un identificador. El resultado es una colección 
/// de IDs (`IdValores`) correspondiente a los elementos seleccionados.
/// 
/// Este componente está desacoplado del ViewModel principal y puede ser enlazado como un control estándar:
///     <IdentificadoresSelect IdValores="{Binding ListaDeIdsSeleccionados}" />
/// 
/// Internamente, se alimenta de servicios usando `ServiceFactory`:
///     - `Service<Identificador>` para obtener los identificadores.
///     - `Service<ElementoJerarquico>` para obtener los valores asociados.
/// </summary>
public partial class IdentificadoresSelect : UserControl

{
    public static readonly DependencyProperty IdValoresProperty =
        DependencyProperty.Register(nameof(IdValores), typeof(ObservableCollection<string>), typeof(IdentificadoresSelect), new FrameworkPropertyMetadata(new ObservableCollection<string>(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIdValoresChanged));

    /// <summary>
    /// Colección de IDs (`string`) correspondientes a los `ElementoJerarquico` seleccionados.
    /// Esta propiedad está enlazada bidireccionalmente y es el principal punto de integración con el exterior.
    /// </summary>
    public ObservableCollection<string> IdValores
    {
        get => (ObservableCollection<string>)GetValue(IdValoresProperty);
        set => SetValue(IdValoresProperty, value);
    }

    public ObservableCollection<SelectorJerarquico> Selectores { get; set; } = new();

    public IdentificadoresSelect()
    {
        InitializeComponent();
        DataContext = this;

        ParseToSelectorJerarquico(Selectores);


    }

    /// <summary>
    /// Detecta cambios en `IdValores` y actualiza las selecciones visuales internas (ComboBox).
    /// </summary>
    private static void OnIdValoresChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is IdentificadoresSelect control)
        {
            control.AplicarSeleccionDesdeIdValores();
        }
    }


    /// <summary>
    /// Aplica los valores de `IdValores` a los ComboBox internos (Selectores),
    /// seleccionando los elementos correspondientes según su ID.
    /// 
    /// Se ejecuta cuando `IdValoresProperty` cambia externamente o al inicializar el componente.
    /// </summary>
    private void AplicarSeleccionDesdeIdValores()
    {
        if (IdValores.Count == 0 || Selectores.Count == 0)
            return;

        foreach (var selector in Selectores)
        {
            // Busca un valor dentro del selector cuyos Id esté en IdValores
            var match = selector.Valores.FirstOrDefault(v => IdValores.Contains(v.Id));

            // Asignar solo si es diferente para evitar notificaciones innecesarias
            if (match != null && selector.Seleccionado?.Id != match.Id)
                selector.Seleccionado = match;
            else if (match == null)
                selector.Seleccionado = null; // Limpia si no hay match
        }
    }



    private void ParseToSelectorJerarquico(ObservableCollection<SelectorJerarquico> selectores)
    {
        selectores.Clear();

        var lista = ((ServiceElementoJerarquico)ServiceFactory.GetService<ElementoJerarquico>()).GetAllIdentificadoresList();

        foreach (var kvp in lista)
        {
            var identificador = kvp.Key;
            var valores = kvp.Value
                .OrderByDescending(e => e.FechaCreacion)
                .ToList();

            var selector = new SelectorJerarquico(identificador, valores);

            // Puedes enganchar el evento si es necesario
            selector.SeleccionadoCambiado += (anterior, nuevo) =>
            {
                if (nuevo == null || !IsLoaded) return;

                if (anterior != null && IdValores.Contains(anterior.Id))
                {
                    IdValores.Remove(anterior.Id);
                }
                if (!IdValores.Contains(nuevo.Id))
                {
                    IdValores.Add(nuevo.Id);
                }
            };

            selectores.Add(selector);
        }

    }
}

/// <summary>
/// Representa la relación entre un Identificador y su lista de valores jerárquicos (`ElementoJerarquico`).
/// Esta clase se usa para alimentar la UI de cada ComboBox dentro del `IdentificadoresSelect`.
/// </summary>
public class SelectorJerarquico : ObservableObject
{
    /// <summary>
    /// Evento que se dispara cada vez que se cambia la selección dentro del ComboBox correspondiente.
    /// Entrega tanto el valor anterior como el nuevo.
    /// </summary>
    public event Action<ElementoJerarquico?, ElementoJerarquico?>? SeleccionadoCambiado;


    public Identificador Identificador { get; }

    public List<ElementoJerarquico> Valores { get; }

    private ElementoJerarquico? _seleccionado;
    /// <summary>
    /// Elemento seleccionado actualmente en este selector. El cambio dispara el evento `SeleccionadoCambiado`.
    /// </summary>
    public ElementoJerarquico? Seleccionado
    {
        get => _seleccionado;
        set
        {
            if (_seleccionado != value)
            {
                var anterior = _seleccionado;
                SetProperty(ref _seleccionado, value);

                // Dispara el evento con anterior y nuevo
                SeleccionadoCambiado?.Invoke(anterior, _seleccionado);
            }
        }
    }

    public SelectorJerarquico(Identificador identificador, List<ElementoJerarquico> valores)
    {
        Identificador = identificador;
        Valores = valores;
    }
}
