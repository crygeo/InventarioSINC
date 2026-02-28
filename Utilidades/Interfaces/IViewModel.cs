using System.Windows.Input;

namespace Utilidades.Interfaces;

public interface IViewModel
{
    ICommand CerrarVentanaCommand {get; set;}
    void CerrarVentana(object ob);
}