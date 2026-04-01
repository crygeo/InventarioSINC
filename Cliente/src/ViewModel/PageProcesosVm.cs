using System.Diagnostics;
using Cliente.Helpers;
using Cliente.Obj;
using Shared.Interfaces.Model;
using Utilidades.Interfaces;
using Utilidades.Mvvm;

namespace Cliente.ViewModel;

public class PageProcesosVm : PageNavegacionVM
{
    private const string Key = "PanelConfig";

    public PageProcesosVm() : base(NavegacionFactory.ObtenerTodosPorIndicador(Key)
            .Cast<IItemNav>()
            .ToList())
    { }
}