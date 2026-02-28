using CommunityToolkit.Mvvm.Input;

namespace Utilidades.Dialogs;

public interface IDialogBase
{
    string TextHeader { get; set; }
    string DialogNameIdentifier { get; set; }
    string DialogOpenIdentifier { get; set; }
    
}

public interface IDialog : IDialogBase
{
    IAsyncRelayCommand AceptarCommand { get; set; }
}

public interface IDialog<TEntity> : IDialogBase
{
    IAsyncRelayCommand<TEntity> AceptarCommand { get; set; }
    IAsyncRelayCommand CancelarCommand { get; set; }
    TEntity Entity { get; set; }
}