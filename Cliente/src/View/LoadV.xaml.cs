using Cliente.src.ViewModel;
using Cliente.src.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Cliente.src.View
{
    /// <summary>
    /// Lógica de interacción para LoadView.xaml
    /// </summary>
    public partial class LoadV : Window
    {
        public LoadV()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                LoadV_Loaded();
                if (DataContext is LoadVM vm)
                {
                    vm.StartCommand.Execute(null);
                }
            };

        }
        private void LoadV_Loaded()
        {
            // Crear la animación de rotación
            DoubleAnimation rotationAnimation = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = TimeSpan.FromSeconds(3), // Tiempo en dar una vuelta
                RepeatBehavior = RepeatBehavior.Forever // Girar infinitamente
            };

            // Aplicar la animación a la transformación de la imagen
            RotateTransform rotateTransform = new RotateTransform();
            ImageRotateTransform.Angle = 0;
            ImageRotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);
        }

    }
}
