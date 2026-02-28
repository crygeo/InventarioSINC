using System.Collections.Generic;

namespace Utilidades.Interfaces;

public interface IBarNavegacion
{
    IItemNav SelectedItemNav { get; set; }
    List<IItemNav> ListItemsNav { get; }
}