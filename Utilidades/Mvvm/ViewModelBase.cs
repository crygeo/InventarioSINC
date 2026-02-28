using System.Windows.Input;
using Utilidades.Interfaces;

namespace Utilidades.Mvvm;

public abstract class ViewModelBase :  NotifyProperty, IViewModel
{
    public ICommand CerrarVentanaCommand {get; set;}

    public ViewModelBase()
    {
        CerrarVentanaCommand = new RelayCommand(CerrarVentana);
    }

    public virtual void CerrarVentana(object ob)
    {
    }
}

