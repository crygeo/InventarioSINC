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
using Cliente.Obj.Model;
using Cliente.Services.Model;
using Shared.Interfaces.Model;
using Shared.Interfaces.Model.Obj;

namespace Cliente.View.Items
{
    /// <summary>
    /// Lógica de interacción para ProveedorSelect.xaml
    /// </summary>
    public partial class ProveedorSelect : UserControl
    {

        public static readonly DependencyProperty ProveedorProperty = DependencyProperty.Register(nameof(Proveedor), typeof(Proveedor), typeof(ProveedorSelect), new PropertyMetadata(null));
        public static readonly DependencyProperty ListProveedorProperty = DependencyProperty.Register(nameof(ListProveedor), typeof(IEnumerable<Proveedor>), typeof(ProveedorSelect), new PropertyMetadata(null));


        public Proveedor? Proveedor
        {
            get => (Proveedor)GetValue(ProveedorProperty);
            set => SetValue(ProveedorProperty, value);
        }

        public IEnumerable<Proveedor> ListProveedor
        {
            get => (IEnumerable<Proveedor>)GetValue(ListProveedorProperty);
            set => SetValue(ListProveedorProperty, value);
        }

        public ProveedorSelect()
        {
            InitializeComponent();

            ListProveedor = ServiceFactory.GetService<Proveedor>().Collection;

        }

        private void OnClearClick(object sender, RoutedEventArgs e)
        {
            Proveedor = null;
        }
    }
}
