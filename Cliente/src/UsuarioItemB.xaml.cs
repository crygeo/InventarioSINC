using Cliente.src.Extencions;
using Cliente.src.Model;
using Cliente.src.Services.Model;
using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Cliente.src.Services;

namespace Cliente.src.View.Items
{
    /// <summary>
    /// Lógica de interacción para UsuarioItemB.xaml
    /// </summary>
    public partial class UsuarioItemB : UserControl
    {
        // Propiedad de Dependencia para el Usuario
        public static readonly DependencyProperty UsuarioProperty = DependencyProperty.Register(nameof(Usuario), typeof(Usuario), typeof(UsuarioItemB), new PropertyMetadata(null, OnUsuarioChanged));
        public static readonly DependencyProperty IsSelectProperty = DependencyProperty.Register(nameof(IsSelect), typeof(bool), typeof(UsuarioItemB), new PropertyMetadata(false));
        //public static readonly DependencyProperty SeleccionarUsuarioCommandProperty = DependencyProperty.Register(nameof(SeleccionarUsuarioCommand), typeof(ICommand), typeof(UsuarioItemB));
        public static readonly DependencyProperty EditarUsuarioCommandProperty = DependencyProperty.Register(nameof(EditarUsuarioCommand), typeof(ICommand), typeof(UsuarioItemB));
        public static readonly DependencyProperty EliminarUsuarioCommandProperty = DependencyProperty.Register(nameof(EliminarUsuarioCommand), typeof(ICommand), typeof(UsuarioItemB));
        public static readonly DependencyProperty CambiarPasswordCommandProperty = DependencyProperty.Register(nameof(CambiarPasswordCommand), typeof(ICommand), typeof(UsuarioItemB));
        public static readonly DependencyProperty AsignarRolCommandProperty = DependencyProperty.Register(nameof(AsignarRolCommand), typeof(ICommand), typeof(UsuarioItemB));
        public static readonly DependencyProperty ListRolesProperty = DependencyProperty.Register(nameof(ListRoles), typeof(ObservableCollection<Rol>), typeof(UsuarioItemB));

        public Usuario Usuario
        {
            get => (Usuario)GetValue(UsuarioProperty);
            set => SetValue(UsuarioProperty, value);
        }

        public bool IsSelect
        {
            get => (bool)GetValue(IsSelectProperty);
            set => SetValue(IsSelectProperty, value);
        }

        // Comando de Selección

        //public ICommand SeleccionarUsuarioCommand
        //{
        //    get => (ICommand)GetValue(SeleccionarUsuarioCommandProperty);
        //    set => SetValue(SeleccionarUsuarioCommandProperty, value);
        //}
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

        public ObservableCollection<Rol> ListRoles
        {
            get => (ObservableCollection<Rol>)GetValue(ListRolesProperty);
            set => SetValue(ListRolesProperty, value);
        }

        public UsuarioItemB()
        {
            InitializeComponent();

            ListRoles = ServiceFactory.GetService<Rol>().Collection.Clone(); // Clona la colección de roles desde el servicio
            //DataContext = this; // 🔥 Establece el DataContext en la misma instancia del control

        }

        private static void OnUsuarioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as UsuarioItemB;

            if (control?.Usuario != null && control.ListRoles != null)
            {
                control.ListRoles.SelectRol(control.Usuario.Roles);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuitem && menuitem.DataContext is Rol rol)
            {
                AsignarRolCommand?.Execute(rol);
            }
        }


    }
}
