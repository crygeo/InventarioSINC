using System;
using Utilidades.Mvvm;

namespace Utilidades.Command;

public class UpdateVMCommand : CommandBase
{
    private ViewModelBase _viewModel;

    public UpdateVMCommand(ViewModelBase viewModel)
    {
        _viewModel = viewModel;
    }

    public override void Execute(object parameter)
    {
        if (parameter is Type str) _viewModel = (ViewModelBase)Activator.CreateInstance(str);
    }
}