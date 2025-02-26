using InventarioSINCliente.src.Model;
using InventarioSINCliente.src.Services;
using InventarioSINCliente.src.View;
using InventarioSINCliente.src.WindowStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Utilidades.Mvvm;
using Utilidades.Services;
using Utilidades.WindowStrategies;

namespace InventarioSINCliente.src.ViewModel
{
    public class LoginVM : ViewModelBase
    {
        protected override ModelBase Model { get; set; } = new LoginM();
        private LoginM LoginM => (LoginM)Model;

        private string _usuario;
        private string _password;
        private bool _rememberMe;

        public string Usuario
        {
            get => _usuario;
            set => SetProperty(ref _usuario, value, nameof(Usuario));
        }
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value, nameof(Password));
        }
        public bool RememberMe
        {
            get => _rememberMe;
            set => SetProperty(ref _rememberMe, value, nameof(RememberMe));
        }

        public ICommand LoginCommand { get; set; }
        private readonly IWindowStrategy MainWindow = new MainWS();

        public LoginVM()
        {
            _password = string.Empty;
            _usuario = string.Empty;
            _rememberMe = false;

            LoginCommand = new RelayCommand(Login);
            CargarDatos();
        }

        private async void Login(object obj)
        {
            // Aqui va la logica de login
            if (await LoginM.AuthService.LoginAsync(Usuario, Password))
            {
                MessageBox.Show("¡Inicio de sesión exitoso!", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                if (RememberMe == true)
                    // Guardar el usuario y contraseña en el almacenamiento local
                    await LoginM.AuthService.GuardarCredenciales(Usuario, Password, RememberMe);
                else
                    // Borrar las credenciales guardadas
                    await LoginM.AuthService.BorrarCredenciales();

                MainWindow.OpenWindow();
                NavigationService.CloseWindow<LoginV>();

            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private async void CargarDatos()
        {
            var dat = await LoginM.AuthService.CargarCredenciales();
            if (dat.Item3)
            {
                Usuario = dat.Item1;
                Password = dat.Item2;
                RememberMe = dat.Item3;
            }
        }
    }
}
