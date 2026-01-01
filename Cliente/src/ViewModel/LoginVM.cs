using System.Windows.Input;
using Cliente.Default;
using Cliente.Obj.Model;
using Cliente.Services;
using Cliente.ServicesHub;
using Cliente.View;
using Cliente.WindowStrategy;
using Utilidades.Dialogs;
using Utilidades.Mvvm;
using Utilidades.Services;
using Utilidades.WindowStrategies;
using static Cliente.Services.AuthService;

namespace Cliente.ViewModel;

public class LoginVM : ViewModelBase
{
    private readonly AuthService _authService;
    private readonly IWindowStrategy MainWindow = new MainWS();
    private string _password;
    private bool _rememberMe;
    private string _usuario;

    public LoginVM()
    {
        _authService = new AuthService();
        _password = string.Empty;
        _usuario = string.Empty;
        _rememberMe = false;

        LoginCommand = new RelayCommand(Login);
        CargarDatos();
    }

    public string Usuario
    {
        get => _usuario;
        set => SetProperty(ref _usuario, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public bool RememberMe
    {
        get => _rememberMe;
        set => SetProperty(ref _rememberMe, value);
    }

    public ICommand LoginCommand { get; set; }

    private async void Login(object obj)
    {
        var res = await DialogService.Instance.MostrarDialogoProgreso(async () =>
        {
            var result = await _authService.LoginAsync(Usuario, Password);
            await DialogService.Instance.ValidarRespuesta(result, DialogDefaults.Login);
            return result;
        }, DialogDefaults.Login);

        if (res.Success)
        {
            if (RememberMe)
                await GuardarCredenciales(Usuario, Password, RememberMe);
            else
                await BorrarCredenciales();
            await MostDialog(res.Message);
        }
    }


    private async Task MostDialog(string msg)
    {
        MessageDialog successDialog = new()
        {
            Message = $"!{msg}¡",
            DialogNameIdentifier = $"Dialog_{Guid.NewGuid():N}",
            DialogOpenIdentifier = "RootDialog"
        };

        await DialogService.Instance.MostrarDialogo(successDialog);

        // 🔥 Inicializar TODOS los Hubs ANTES de abrir la ventana principal
        await HubInitializer.InitializeAsync();

        // 🔥 Ahora sí abrir la ventana principal
        MainWindow.OpenWindow();

        // Cerrar login
        NavigationService.CloseWindow<LoginV>();
    }


    private async void CargarDatos()
    {
        var dat = await CargarCredenciales();
        if (dat.Item3)
        {
            Usuario = dat.Item1;
            Password = dat.Item2;
            RememberMe = dat.Item3;
        }
    }
    
    protected override void UpdateChanged()
    {
    }
}