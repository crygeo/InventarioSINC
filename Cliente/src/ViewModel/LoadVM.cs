using System.Windows.Input;
using Cliente.Obj.Model;
using Cliente.Services;
using Cliente.ServicesHub;
using Cliente.View;
using Cliente.WindowStrategy;
using Utilidades.Mvvm;
using Utilidades.Services;
using Utilidades.WindowStrategies;

namespace Cliente.ViewModel;

public class LoadVM : ViewModelBase
{
    private readonly AuthService _authService;
    private readonly IWindowStrategy _loginWindow = new LoginWS();
    private readonly IWindowStrategy _mainWindow = new MainWS();
    private string _logText = string.Empty;

    public LoadVM()
    {
        _authService = new AuthService();
        LogText = "Conectando...";
        StartCommand = new RelayCommand(async _ => await VerificarConexionAsync());
    }

    public string LogText
    {
        get => _logText;
        set => SetProperty(ref _logText, value);
    }

    public ICommand StartCommand { get; }

    private async Task VerificarConexionAsync()
    {
        var inicializado = false;

        while (!inicializado)
        {
            inicializado = await _authService.VerificarServidorAsync();
            if (!inicializado)
            {
                LogText = "Error de conexión";
                await Task.Delay(3000);

                for (var i = 5; i > 0; i--)
                {
                    LogText = $"Reconectando en {i} segundos";
                    await Task.Delay(1000);
                }

                LogText = "Conectando...";
            }
        }

        LogText = "Iniciando sesión...";
        var tiempoSesion = await _authService.VerificarLogin();

        // 🔥 1) SI HAY SESIÓN → iniciar hubs ANTES de abrir Main
        if (tiempoSesion != TimeSpan.Zero)
        {
            LogText = "Conectando servicios...";
            await HubInitializer.InitializeAsync();

            _mainWindow.OpenWindow();
            NavigationService.CloseWindow<LoadV>();
            return;
        }

        // 🔥 2) SI NO HAY SESIÓN → abrir Login sin iniciar hubs
        _loginWindow.OpenWindow();
        NavigationService.CloseWindow<LoadV>();
    }
    


    protected override void UpdateChanged()
    {
    }
}