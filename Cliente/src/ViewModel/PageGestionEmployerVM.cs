using Cliente.Helpers;
using Cliente.Obj;
using Utilidades.Interfaces;
using Utilidades.Mvvm;

namespace Cliente.ViewModel;

public class PageGestionEmployerVM : PageNavegacionVM
{
    private const string Key = "PageGestionEmployer";

    public PageGestionEmployerVM()
        : base(NavegacionFactory.ObtenerTodosPorIndicador(Key)
            .Cast<IItemNav>()
            .ToList())
    { }
}