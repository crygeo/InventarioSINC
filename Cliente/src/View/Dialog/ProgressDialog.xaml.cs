using System.Windows.Controls;
using Utilidades.Interfaces;

namespace Cliente.View.Dialog;
/// <summary>
/// Interaction logic for SampleProgressDialog.xaml
/// </summary>
public partial class ProgressDialog : UserControl, IDialog
{
    public ProgressDialog()
    {
        InitializeComponent();
    }

    public string DialogNameIdentifier { get; set; }
    public required string DialogOpenIdentifier { get; set; }
}
