using System.Collections.ObjectModel;
using Cliente.Default;
using Cliente.Obj.Model;
using Cliente.Services.Model;

namespace Cliente.ViewModel.Model;

public partial class PageGrupoVM : ViewModelServiceBase<Grupo>
{
    private Seccion? _seccionPadre;
    private ServiceBase<Empleado> ServiceEmpleados => ServiceFactory.GetService<Empleado>();
    public ServiceGrupo ServiceGrupo => (ServiceGrupo)ServicioBase;

    public Seccion? SeccionPadre
    {
        get => _seccionPadre;
        set
        {
            SetProperty(ref _seccionPadre, value);

            CrearEntityCommand.NotifyCanExecuteChanged();
        }
    }

    protected override bool CanCrearEntity => SeccionPadre is not null;
    protected override bool CanEditarEntity => EntitySelect is not null;
    protected override bool CanEliminarEntity => EntitySelect is not null;

    protected async override Task InitAsync()
    {
        PageIndex = 0;
        PageSize = int.MaxValue;
        
        await base.InitAsync();
    }

    public override Task CreateAsync()
    {
        if (SeccionPadre is not null && SeccionPadre.EsGrupo)
            return base.CreateAsync();

        DialogServiceI.MensajeQueue.Enqueue("Esta sección no permite grupos.");
        return Task.CompletedTask;
    }

    public async override Task ConfirmarCrearEntityAsync(Grupo? entity)
    {
        if (entity == null || SeccionPadre == null)
            return;

        await DialogServiceI.MostrarDialogoProgreso(async () =>
        {
            var result = await ServiceGrupo.CreateInSeccionAsync(entity, SeccionPadre?.Id);
            result.ObjInteration = typeof(Grupo);
            await DialogServiceI.ValidarRespuesta(result);
            return result;
        }, DialogDefaults.Progress);
    }

    protected override void UpdateChanged()
    {
        base.UpdateChanged();
    }
}