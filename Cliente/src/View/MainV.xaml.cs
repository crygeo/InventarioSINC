using System.Windows;
using Cliente.Services;

namespace Cliente.View;

/// <summary>
/// Lógica de interacción para MainV.xaml
/// </summary>
public partial class MainV : Window
{
    public MainV()
    {
        InitializeComponent();
        SnackbarThree.MessageQueue = DialogService.Instance.MensajeQueue;
    }
}