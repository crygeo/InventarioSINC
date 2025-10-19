using System.Windows.Input;
using Cliente.Services;
using Cliente.View;
using Cliente.View.Dialog;
using Cliente.WindowStrategy;
using Utilidades.Dialogs;
using Utilidades.Interfaces;
using Utilidades.Mvvm;
using Utilidades.Services;
using Utilidades.WindowStrategies;
using static Cliente.Services.AuthService;

namespace Cliente.ViewModel;

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
        IResultResponse<TokenResponse> res = await DialogService.Instance.MostrarDialogoProgreso(async () =>
        {
            var result = await _authService.LoginAsync(Usuario, Password);
            await DialogService.Instance.ValidarRespuesta(result, "RootDialog");
            return result;
        }, "RootDialog");

        if (res.Success)
        {
            if (RememberMe)
                await AuthService.GuardarCredenciales(Usuario, Password, RememberMe);
            else
                await AuthService.BorrarCredenciales();
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

        MainWindow.OpenWindow();
        NavigationService.CloseWindow<LoginV>();
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
    }
}