using Cliente.src.Model;
using System;
using System.Collections;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cliente.src.View.Items
{

    /// <summary>
    /// Lógica de interacción para UsuarioList.xaml
    /// </summary>
    public partial class UsuarioList : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(UsuarioList));
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(object), typeof(UsuarioList));

        public static readonly DependencyProperty EditarUsuarioCommandProperty = DependencyProperty.Register(nameof(EditarUsuarioCommand), typeof(ICommand), typeof(UsuarioList));
        public static readonly DependencyProperty EliminarUsuarioCommandProperty =DependencyProperty.Register(nameof(EliminarUsuarioCommand), typeof(ICommand), typeof(UsuarioList));
        public static readonly DependencyProperty CambiarPasswordCommandProperty = DependencyProperty.Register(nameof(CambiarPasswordCommand), typeof(ICommand), typeof(UsuarioList));
        public static readonly DependencyProperty AsignarRolCommandProperty = DependencyProperty.Register(nameof(AsignarRolCommand), typeof(ICommand), typeof(UsuarioList));



        public ICommand EditarUsuarioCommand
        {
            get => (ICommand)GetValue(EditarUsuarioCommandProperty);
            set => SetValue(EditarUsuarioCommandProperty, value);
        }

        public ICommand EliminarUsuarioCommand
        {
            get => (ICommand)GetValue(EliminarUsuarioCommandProperty);
            set => SetValue(EliminarUsuarioCommandProperty, value);
        }

        public ICommand CambiarPasswordCommand
        {
            get => (ICommand)GetValue(CambiarPasswordCommandProperty);
            set => SetValue(CambiarPasswordCommandProperty, value);
        }

         public ICommand AsignarRolCommand
 {
     get => (ICommand)GetValue(AsignarRolCommandProperty);
     set => SetValue(AsignarRolCommandProperty, value);
 }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        public object Item
        {
            get => (object)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public UsuarioList()
        {
            InitializeComponent();
            //DataContext = this;
        }

        private void ListBoxItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem item && Keyboard.IsKeyDown(Key.LeftShift))
            {
                item.IsSelected = !item.IsSelected;
                e.Handled = true; // Evita que WPF cambie la selección predeterminada
            }
        }

    }
}
