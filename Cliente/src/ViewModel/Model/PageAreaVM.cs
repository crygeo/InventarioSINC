using System.Collections.ObjectModel;
using Cliente.Obj;
using Cliente.Obj.Model;
using Cliente.Services.Model;

namespace Cliente.ViewModel.Model;

public partial class PageAreaVM : ViewModelServiceBase<Area>
{
    public ObservableCollection<TreeNodeBase> TreeNode { get; } = new();
    private TreeNodeBase? _selectedTreeNode;

    public TreeNodeBase? SelectedTreeNode
    {
        get => _selectedTreeNode;
        set
        {
            SetProperty(ref _selectedTreeNode, value);
            OnTreeNodeSelected(value);
        }
    }


    public PageTurnoVM PageTurnoVm { get; }
    public PageSeccionVM PageSeccionVm { get; }
    public PageGrupoVM PageGrupoVm { get; }

    public ObservableCollection<Empleado> Empleados { get; } = new();

    protected override bool CanCrearEntity => true;
    protected override bool CanEditarEntity => EntitySelect is not null;
    protected override bool CanEliminarEntity => EntitySelect is not null;


    public PageAreaVM()
    {
        PageTurnoVm = new PageTurnoVM();
        PageSeccionVm = new PageSeccionVM();
        PageGrupoVm = new PageGrupoVM();
    }

    protected override async Task InitAsync()
    {
        PageIndex = 0;
        PageSize = int.MaxValue;

        ServicioBase.HubService.OnCreated += HubServiceOnOnCreated;
        ServicioBase.HubService.OnUpdated += HubServiceOnOnCreated;
        ServicioBase.HubService.OnDeleted += HubServiceOnOnCreated;


        async void HubServiceOnOnCreated(Object obj)
        {
            await LoadTreeAsync();
        }

        await base.InitAsync();
        _ = LoadTreeAsync();
    }


    private async Task LoadTreeAsync()
    {
        try
        {
            await CreateTreeNode();
        }
        catch (Exception ex)
        {
            DialogServiceI.MensajeQueue.Enqueue(":Error cargando el árbol: " + ex.Message);
        }
    }


    private async Task CreateTreeNode()
    {
        TreeNode.Clear();

        var serviceTurno = PageTurnoVm.ServiceTurno;
        var serviceSecciones = PageSeccionVm.ServiceSeccion;
        var serviceGrupos = PageGrupoVm.ServiceGrupo;

        foreach (var area in Entitys)
        {
            var areaNode = new AreaNode
            {
                Id = area.Id,
                Nombre = area.Nombre,
                Area = area,

                AddCommand = PageTurnoVm.CrearEntityCommand,
                EditCommand = EditarEntityCommand,
                DeleteCommand = EliminarEntityCommand
            };

            TreeNode.Add(areaNode);

            // 👉 Ceder el control a la UI
            await Task.Yield();

            foreach (var turnoId in area.TurnoIds)
            {
                var turnoResp = await serviceTurno.GetByIdAsync(turnoId);
                if (!turnoResp.Success)
                    continue;

                var turno = turnoResp.EntityGet;

                var turnoNode = new TurnoNode
                {
                    Id = turno.Id,
                    Nombre = turno.Nombre,
                    Turno = turno,
                    Parent = areaNode,

                    AddCommand = PageSeccionVm.CrearEntityCommand,
                    EditCommand = PageTurnoVm.EditarEntityCommand,
                    DeleteCommand = PageTurnoVm.EliminarEntityCommand
                };

                areaNode.Children.Add(turnoNode);
                await Task.Yield();

                foreach (var seccionId in turno.SeccionIds)
                {
                    var seccionResp = await serviceSecciones.GetByIdAsync(seccionId);
                    if (!seccionResp.Success)
                        continue;

                    var seccion = seccionResp.EntityGet;

                    var seccionNode = new SeccionNode
                    {
                        Id = seccion.Id,
                        Nombre = seccion.Nombre,
                        Seccion = seccion,
                        Parent = turnoNode,

                        AddCommand = PageGrupoVm.CrearEntityCommand,
                        EditCommand = PageSeccionVm.EditarEntityCommand,
                        DeleteCommand = PageSeccionVm.EliminarEntityCommand
                    };

                    turnoNode.Children.Add(seccionNode);
                    await Task.Yield();

                    if (!seccion.EsGrupo)
                        continue;

                    foreach (var grupoId in seccion.GrupoIds)
                    {
                        var grupoResp = await serviceGrupos.GetByIdAsync(grupoId);
                        if (!grupoResp.Success)
                            continue;

                        var grupo = grupoResp.EntityGet;

                        var grupoNode = new GrupoNode
                        {
                            Id = grupo.Id,
                            Nombre = grupo.Nombre,
                            Grupo = grupo,
                            Parent = seccionNode,

                            EditCommand = PageGrupoVm.EditarEntityCommand,
                            DeleteCommand = PageGrupoVm.EliminarEntityCommand
                        };

                        seccionNode.Children.Add(grupoNode);
                        await Task.Yield();
                    }
                }
            }
        }
    }


    private void OnTreeNodeSelected(TreeNodeBase? node)
    {
        // 🔥 LIMPIAR CONTEXTO
        PageTurnoVm.AreaPadre = null;
        PageSeccionVm.TurnoPadre = null;
        PageGrupoVm.SeccionPadre = null;

        ClearEntitySelections();

        Area? area = null;
        Turno? turno = null;
        Seccion? seccion = null;

        switch (node)
        {
            case AreaNode a:
                area = a.Area;
                EntitySelect = area;
                break;

            case TurnoNode t:
                area = FindAreaFromTurnoNode(t);
                turno = t.Turno;
                PageTurnoVm.EntitySelect = turno;
                break;

            case SeccionNode s:
                turno = FindTurnoFromSeccionNode(s);
                seccion = s.Seccion;
                PageSeccionVm.EntitySelect = seccion;
                break;

            case GrupoNode g:
                seccion = FindSeccionFromGrupo(g);
                PageGrupoVm.EntitySelect = g.Grupo;
                break;
        }

        // 🔥 REASIGNAR CONTEXTO
        PageTurnoVm.AreaPadre = area;
        PageSeccionVm.TurnoPadre = turno;
        PageGrupoVm.SeccionPadre = seccion;
    }


    private void ClearEntitySelections()
    {
        EntitySelect = null;
        PageTurnoVm.EntitySelect = null;
        PageSeccionVm.EntitySelect = null;
        PageGrupoVm.EntitySelect = null;
    }

    private static Area FindAreaFromTurnoNode(TurnoNode node)
    {
        return ((AreaNode)node.Parent!).Area;
    }

    private static Turno FindTurnoFromSeccionNode(SeccionNode node)
    {
        return ((TurnoNode)node.Parent!).Turno;
    }

    private static Seccion FindSeccionFromGrupo(GrupoNode node)
    {
        return ((SeccionNode)node.Parent!).Seccion;
    }


    protected override void UpdateChanged()
    {
        base.UpdateChanged();
    }
}