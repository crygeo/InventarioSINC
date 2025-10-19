using Cliente.Services.Model;
using Shared.ObjectsResponse;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Utilidades.Interfaces;
using Utilidades.Dialogs;

namespace Cliente.View.Dialog;

/// <summary>
/// Lógica de interacción para MessageDialog.xaml
/// </summary>
public partial class MessageDialogError : UserControl, IDialogBase
{
    public static readonly DependencyProperty ErrorResponseProperty = DependencyProperty.Register(nameof(ErrorResponse), typeof(IResultResponse), typeof(MessageDialogError));
    private string _textHeader;

    public IResultResponse ErrorResponse
    {
        get => (IResultResponse)GetValue(ErrorResponseProperty);
        set => SetValue(ErrorResponseProperty, value);
    }

    public MessageDialogError()
    {
        InitializeComponent();
    }

    private void CopiarError_Click(object sender, RoutedEventArgs e)
    {
        Clipboard.SetText(ErrorResponse.GetErrorFormat());
        SnackbarCopiado?.MessageQueue?.Enqueue("Error copiado al portapapeles");

    }

    private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount != 2) return;

        Clipboard.SetText(ErrorResponse.GetErrorFormat());
        SnackbarCopiado?.MessageQueue?.Enqueue("Error copiado al portapapeles");
    }


    public ResultResponse<T> GetErrorResponse<T>() => (ResultResponse<T>)ErrorResponse!;
    public void SetErrorResponse<T>(ResultResponse<T> value) => ErrorResponse = value;


    public string TextHeader
    {
        get => _textHeader;
        set => _textHeader = value;
    }

    public string DialogNameIdentifier { get; set; } = $"Dialog_{Guid.NewGuid():N}";
    public required string DialogOpenIdentifier { get; set; }
}