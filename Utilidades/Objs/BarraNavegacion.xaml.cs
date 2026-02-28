using System.Windows;
using System.Windows.Controls;
using Utilidades.Interfaces;

namespace Utilidades.Objs;

/// <summary>
///     Lógica de interacción para BarraNavegacion.xaml
/// </summary>
public partial class BarraNavegacion : UserControl
{
    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(nameof(ViewModel), typeof(IBarNavegacion), typeof(BarraNavegacion),
            new PropertyMetadata(null));

    public static readonly DependencyProperty OrientacionProperty =
        DependencyProperty.Register(nameof(Orientacion), typeof(Orientation), typeof(BarraNavegacion),
            new PropertyMetadata(Orientation.Horizontal, OnOrientacionChanged));

    public BarraNavegacion()
    {
        InitializeComponent();
    }

    public Orientation Orientacion
    {
        get => (Orientation)GetValue(OrientacionProperty);
        set => SetValue(OrientacionProperty, value);
    }

    public IBarNavegacion ViewModel
    {
        get => (IBarNavegacion)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    private static void OnOrientacionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (BarraNavegacion)d;
        control.ActualizarOrientacion();
    }

    private void ActualizarOrientacion()
    {
        if (Lista != null)
        {
            if (Orientacion == Orientation.Horizontal)
            {
                Lista.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
                Lista.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
            }
            else
            {
                Lista.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
                Lista.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
            }
        }
    }
}