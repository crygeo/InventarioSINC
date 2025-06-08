using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Cliente.ViewModel;

namespace Cliente.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class LoginV : Window
{

    // Cambiar el constructor para que sea async
    public LoginV()
    {
        // Llamar al método async y esperar la verificación antes de inicializar la UI
        InitializeComponent();
        // Registrar el evento PreviewKeyDown a nivel de ventana
        this.PreviewKeyDown += Window_PreviewKeyDown;

    }

    // Evento que captura las teclas presionadas en la ventana
    private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        // Si se presiona la tecla Enter
        if (e.Key == Key.Enter)
        {
            if (Keyboard.FocusedElement == PasswordBox)
            {
                var dt = (LoginVM)this.DataContext;
                if (dt != null) dt.LoginCommand.Execute(null);
            }
        }
    }

    private void Button_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (sender is Button button)
            {
                // Evitar que la tecla Enter realice su comportamiento predeterminado
                e.Handled = true;
                button.Command?.Execute(null);
            }
        }
    }
}