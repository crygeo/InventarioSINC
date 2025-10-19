using Cliente.ViewModel;
using Cliente.ViewModel.Model;
using Utilidades.Command;
using Utilidades.Mvvm;

namespace Cliente.Command;

public class ChangedPageC : CommandBase
{
    private Action<ViewModelBase> _setPageSelectViewModel;

    public ChangedPageC(Action<ViewModelBase> setPageSelectViewModel)
    {
        _setPageSelectViewModel = setPageSelectViewModel;
    }

    public override void Execute(object parameter)
    {
        if (parameter is string namePage)
        {
            ViewModelBase newPage = namePage switch
            {
                "Usuarios" => new PageUsuarioVM(), // Página de usuarios
                "Roles" => new PageRolesVM(), // Página de roles
                _ => new PageUsuarioVM(), // Default
            };

            // Cambiar la propiedad PageSelectViewModel
            _setPageSelectViewModel(newPage);
        }

    }
}