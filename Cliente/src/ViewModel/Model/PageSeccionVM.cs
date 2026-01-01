using System.Collections.ObjectModel;
using Cliente.Default;
using Cliente.Obj.Model;
using Cliente.Services.Model;
using CommunityToolkit.Mvvm.Input;

namespace Cliente.ViewModel.Model;

public partial class PageSeccionVM : ViewModelServiceBase<Seccion>
{
    public ServiceSeccion ServiceSeccion => (ServiceSeccion)ServicioBase;

    private Turno? _turnoPadre;

    public Turno? TurnoPadre
    {
        get => _turnoPadre;
        set
        {
            SetProperty(ref _turnoPadre, value);
            CrearEntityCommand.NotifyCanExecuteChanged();
        }
    }

    protected override bool CanCrearEntity => TurnoPadre is not null;
    protected override bool CanEditarEntity => EntitySelect is not null;
    protected override bool CanEliminarEntity => EntitySelect is not null;

    protected async override Task InitAsync()
    {
        PageIndex = 0;
        PageSize = int.MaxValue;
        
        await base.InitAsync();
    }

    public async override Task ConfirmarCrearEntityAsync(Seccion? entity)
    {
        if (entity == null || TurnoPadre == null)
            return;

        await DialogServiceI.MostrarDialogoProgreso(async () =>
        {
            var result = await ServiceSeccion.CreateInTurnoAsync(entity, TurnoPadre?.Id);
            result.ObjInteration = typeof(Seccion);
            await DialogServiceI.ValidarRespuesta(result);
            return result;
        }, DialogDefaults.Progress);
    }


    protected override void UpdateChanged()
    {
        base.UpdateChanged();
    }
}