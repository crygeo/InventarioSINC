using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using Utilidades.Interfaces;
using Utilidades.Dialogs;

namespace Cliente.View.Dialog;
/// <summary>
/// Interaction logic for SampleProgressDialog.xaml
/// </summary>
public partial class ProgressDialog : UserControl, IDialogBase
{
    private string _textHeader;

    public ProgressDialog()
    {
        InitializeComponent();
    }

    public string TextHeader
    {
        get => _textHeader;
        set => _textHeader = value;
    }

    public string DialogNameIdentifier { get; set; }
    public required string DialogOpenIdentifier { get; set; }

    
}
