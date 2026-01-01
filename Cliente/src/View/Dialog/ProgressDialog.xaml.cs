using System.Windows.Controls;
using Utilidades.Dialogs;

namespace Cliente.View.Dialog;

/// <summary>
///     Interaction logic for SampleProgressDialog.xaml
/// </summary>
public partial class ProgressDialog : UserControl, IDialogBase
{
    public ProgressDialog()
    {
        InitializeComponent();
    }

    public string TextHeader { get; set; }

    public string DialogNameIdentifier { get; set; }
    public required string DialogOpenIdentifier { get; set; }
}