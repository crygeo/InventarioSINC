using Cliente.src.Model;
using System;
using System.Collections.Generic;
using System.IO;
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
using Utilidades.Factory;
using Utilidades.Extencions;

namespace Cliente.src.View.Items
{
    /// <summary>
    /// Lógica de interacción para UsuarioItemDetall.xaml
    /// </summary>
    public partial class RolDialog : UserControl
    {
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(Rol), typeof(RolDialog));
        public static readonly DependencyProperty ListPermsProperty = DependencyProperty.Register(nameof(ListPerms), typeof(List<Nodos>), typeof(RolDialog));
        public static readonly DependencyProperty AceptedCommandProperty = DependencyProperty.Register(nameof(AceptedCommand), typeof(ICommand), typeof(RolDialog));
        public static readonly DependencyProperty TextHeaderProperty = DependencyProperty.Register(nameof(TextHeader), typeof(string), typeof(RolDialog));

        public Rol Item
        {
            get => (Rol)GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }

        public List<Nodos> ListPerms
        {
            get => (List<Nodos>)GetValue(ListPermsProperty);
            set => SetValue(ListPermsProperty, value);
        }

        public ICommand AceptedCommand
        {
            get => (ICommand)GetValue(AceptedCommandProperty);
            set => SetValue(AceptedCommandProperty, value);
        }

        public string TextHeader
        {
            get => (string)GetValue(TextHeaderProperty);
            set => SetValue(TextHeaderProperty, value);
        }

        public RolDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var list = ListPerms.ObtenerSeleccionados().DescostruirArbol();
            Item.Permisos = list;

            AceptedCommand.Execute(Item);
            
        }
    }
}
