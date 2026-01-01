using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Cliente.ViewModel;

namespace Cliente.View;

/// <summary>
///     Lógica de interacción para LoadView.xaml
/// </summary>
public partial class LoadV : Window
{
    public LoadV()
    {
        InitializeComponent();
        Loaded += (s, e) =>
        {
            LoadV_Loaded();
            if (DataContext is LoadVM vm) vm.StartCommand.Execute(null);
        };
    }

    private void LoadV_Loaded()
    {
        // Crear la animación de rotación
        var rotationAnimation = new DoubleAnimation
        {
            From = 0,
            To = 360,
            Duration = TimeSpan.FromSeconds(3), // Tiempo en dar una vuelta
            RepeatBehavior = RepeatBehavior.Forever // Girar infinitamente
        };

        // Aplicar la animación a la transformación de la imagen
        var rotateTransform = new RotateTransform();
        ImageRotateTransform.Angle = 0;
        ImageRotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);
    }
}