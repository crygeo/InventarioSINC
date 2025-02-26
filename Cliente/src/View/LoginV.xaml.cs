using InventarioSINCliente.src.Services;
using System.IO.IsolatedStorage;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utilidades.Controls;

namespace InventarioSINCliente.src.View
{
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
                // Evitar que la tecla Enter realice su comportamiento predeterminado
                e.Handled = true;

                // Intentamos mover el foco al siguiente control en la secuencia de tabulación
                var request = new TraversalRequest(FocusNavigationDirection.Next);

                // Intentar mover el foco al siguiente control
                UIElement focusedElement = Keyboard.FocusedElement as UIElement;
                if (focusedElement != null)
                {
                    focusedElement.MoveFocus(request);
                }
            }
        }


    }

}