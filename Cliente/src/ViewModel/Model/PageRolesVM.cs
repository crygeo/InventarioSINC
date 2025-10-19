using Cliente.Obj.Model;
using Cliente.Services;
using Cliente.Services.Model;
using Cliente.View.Dialog;
using CommunityToolkit.Mvvm.Input;
using Shared.Extensions;
using Utilidades.Extencions;

namespace Cliente.ViewModel.Model;

public class PageRolesVM : ViewModelServiceBase<Rol>
{
    public ServiceRol ServiceRol => (ServiceRol)ServicioBase;
   
    protected override void UpdateChanged()
    {
    }
}