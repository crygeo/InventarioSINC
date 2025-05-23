using Cliente.src.Services;
using Cliente.src.View;
using Cliente.src.View.Dialog;
using Cliente.src.WindowStrategy;
using MaterialDesignThemes.Wpf;
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

namespace Cliente.src.ViewModel
{
    public class LoginVM : ViewModelBase
    {
        private AuthService _authService;
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
            _authService = new AuthService();
            _password = string.Empty;
            _usuario = string.Empty;
            _rememberMe = false;

            LoginCommand = new RelayCommand(Login);
            CargarDatos();
        }

        private async void Login(object obj)
        {
            ProgressDialog progressDialog = new();
            MessageDialog successDialog = new()
            {
                Message = "¡Inicio de sesión exitoso!"
            };


            _ = DialogHost.Show(progressDialog, "RootDialog");

            try
            {
                var result = await _authService.LoginAsync(Usuario, Password);
                // Lógica de login
                if (result.Item1)
                {
                    if (RememberMe)
                        await AuthService.GuardarCredenciales(Usuario, Password, RememberMe);
                    else
                        await AuthService.BorrarCredenciales();

                    // Cerrar ProgressDialog antes de mostrar el mensaje de éxito
                    DialogHost.Close("RootDialog");

                    // Mostrar mensaje de éxito
                    await DialogHost.Show(successDialog, "RootDialog");

                    // Abrir la ventana principal y cerrar la de login
                    MainWindow.OpenWindow();
                    NavigationService.CloseWindow<LoginV>();
                }
                else
                {
                    // Cerrar ProgressDialog antes de mostrar el mensaje de error
                    DialogHost.Close("RootDialog");

                    var dialogError = new MessageDialog
                    {
                        Message = result.Item2
                    };
                    // Mostrar mensaje de error
                    await DialogHost.Show(dialogError, "RootDialog");
                }
            }
            catch (Exception ex)
            {
                DialogHost.Close("RootDialog");

                MessageDialog exceptionDialog = new()
                {
                    Message = $"Error al iniciar sesión: {ex.Message}"
                };
                await DialogHost.Show(exceptionDialog, "RootDialog");
            }
        }


        private async void CargarDatos()
        {
            var dat = await AuthService.CargarCredenciales();
            if (dat.Item3)
            {
                Usuario = dat.Item1;
                Password = dat.Item2;
                RememberMe = dat.Item3;
            }
        }

        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }
}
