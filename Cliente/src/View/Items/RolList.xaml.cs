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
    /// Lógica de interacción para RolList.xaml
    /// </summary>
    public partial class RolList : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(RolList));
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(object), typeof(RolList));

        public static readonly DependencyProperty EditarItemCommandProperty = DependencyProperty.Register(nameof(EditarItemCommand), typeof(ICommand), typeof(RolList));
        public static readonly DependencyProperty EliminarItemCommandProperty =DependencyProperty.Register(nameof(EliminarItemCommand), typeof(ICommand), typeof(RolList));



        public ICommand EditarItemCommand
        {
            get => (ICommand)GetValue(EditarItemCommandProperty);
            set => SetValue(EditarItemCommandProperty, value);
        }

        public ICommand EliminarItemCommand
        {
            get => (ICommand)GetValue(EliminarItemCommandProperty);
            set => SetValue(EliminarItemCommandProperty, value);
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

        public RolList()
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
