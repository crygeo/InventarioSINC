using Cliente.src.Services;
using Cliente.src.View;
using Cliente.src.WindowStrategy;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Utilidades.Mvvm;
using Utilidades.Services;
using Utilidades.WindowStrategies;

namespace Cliente.src.ViewModel
{
    public class LoadVM : ViewModelBase
    {

        private readonly AuthService _authService;
        private string _logText = string.Empty;
        private IWindowStrategy _mainWindow = new MainWS();
        private IWindowStrategy _loginWindow = new LoginWS();

        public string LogText
        {
            get => _logText;
            set => SetProperty(ref _logText, value, nameof(LogText));
        }

        public ICommand StartCommand { get; }

        public LoadVM()
        {
            _authService = new AuthService();
            LogText = "Conectando...";
            StartCommand = new RelayCommand(async _ => await VerificarConexionAsync());
        }

        private async Task VerificarConexionAsync()
        {
            bool inicializado = false;

            while (!inicializado)
            {
                inicializado = await _authService.VerificarServidorAsync();
                if (!inicializado)
                {
                    LogText = "Error de conexión";
                    await Task.Delay(3000);

                    for (int i = 5; i > 0; i--)
                    {
                        LogText = $"Reconectando en {i} segundos";
                        await Task.Delay(1000);
                    }

                    LogText = "Conectando...";
                }
            }

            LogText = "Iniciando sesion.";
            var initLogin = await _authService.VerificarLogin();
            if (initLogin == TimeSpan.Zero)
            {
                // No hay sesión activa
                _loginWindow.OpenWindow();
            }
            else
            {
                // Sesión activa
                _mainWindow.OpenWindow();
            }

            NavigationService.CloseWindow<LoadV>();
        }

        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }
}
