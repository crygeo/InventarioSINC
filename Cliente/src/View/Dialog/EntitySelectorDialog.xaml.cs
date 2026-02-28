using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using Utilidades.Dialogs;

namespace Cliente.View.Dialog;

public partial class EntitySelectorDialog : UserControl, IDialog
{
    private string _textHeader;
    private string _dialogNameIdentifier;
    private string _dialogOpenIdentifier;
    private IAsyncRelayCommand _aceptarCommand;

    public EntitySelectorDialog()
    {
        InitializeComponent();
    }

    public string TextHeader
    {
        get => _textHeader;
        set => _textHeader = value;
    }

    public string DialogNameIdentifier
    {
        get => _dialogNameIdentifier;
        set => _dialogNameIdentifier = value;
    }

    public string DialogOpenIdentifier
    {
        get => _dialogOpenIdentifier;
        set => _dialogOpenIdentifier = value;
    }

    public IAsyncRelayCommand AceptarCommand
    {
        get => _aceptarCommand;
        set => _aceptarCommand = value;
    }
}