using System.Collections.ObjectModel;
using Cliente.Default;
using Cliente.Obj.Model;
using Cliente.Services.Model;
using CommunityToolkit.Mvvm.Input;

namespace Cliente.ViewModel.Model;

public partial class PageTurnoVM : ViewModelServiceBase<Turno>
{
    public ServiceTurno ServiceTurno => (ServiceTurno)ServicioBase;
    private Area? _areaPadre;
    public Area? AreaPadre
    {
        get => _areaPadre;
        set
        {
            SetProperty(ref _areaPadre, value);
            CrearEntityCommand.NotifyCanExecuteChanged();
        }
    }
    
    protected override bool CanCrearEntity => AreaPadre is not null;
    protected override bool CanEditarEntity => EntitySelect is not null;
    protected override bool CanEliminarEntity => EntitySelect is not null;

    protected async override Task InitAsync()
    {
        PageIndex = 0;
        PageSize = int.MaxValue;
        
        await base.InitAsync();
    }

    public async override Task ConfirmarCrearEntityAsync(Turno? entity)
    {
        if (entity == null || AreaPadre == null)
            return;
        
        await DialogServiceI.MostrarDialogoProgreso(async () =>
        {
            var result = await ServiceTurno.CreateInAreaAsync(entity, AreaPadre.Id);
            result.ObjInteration = typeof(Turno);
            await DialogServiceI.ValidarRespuesta(result);
            return result;
        }, DialogDefaults.Progress);
    }

    protected override void UpdateChanged()
    {
        base.UpdateChanged();
    }
}