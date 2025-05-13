using Cliente.src.Model;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cliente.src.View.Items
{
    /// <summary>
    /// Lógica de interacción para ItemItemB.xaml
    /// </summary>
    public partial class RoltemB : UserControl
    {
        // Propiedad de Dependencia para el Item
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(Rol), typeof(RoltemB));
        public static readonly DependencyProperty IsSelectProperty = DependencyProperty.Register(nameof(IsSelect), typeof(bool), typeof(RoltemB), new PropertyMetadata(false));
        public static readonly DependencyProperty EditarItemCommandProperty = DependencyProperty.Register(nameof(EditarItemCommand), typeof(ICommand), typeof(RoltemB));
        public static readonly DependencyProperty EliminarItemCommandProperty = DependencyProperty.Register(nameof(EliminarItemCommand), typeof(ICommand), typeof(RoltemB));
        public Rol Item
        {
            get => (Rol)GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }

        public bool IsSelect
        {
            get => (bool)GetValue(IsSelectProperty);
            set => SetValue(IsSelectProperty, value);
        }

        // Comando de Selección

        //public ICommand SeleccionarItemCommand
        //{
        //    get => (ICommand)GetValue(SeleccionarItemCommandProperty);
        //    set => SetValue(SeleccionarItemCommandProperty, value);
        //}
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

        public RoltemB()
        {
            InitializeComponent();

        }
    }
}
