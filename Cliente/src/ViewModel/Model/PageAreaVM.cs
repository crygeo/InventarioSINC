using System.Collections.ObjectModel;
using Cliente.Obj;
using Cliente.Obj.Model;
using Cliente.Services.Model;
using CommunityToolkit.Mvvm.Input;
using Shared.Interfaces.Model;

namespace Cliente.ViewModel.Model;

public partial class PageAreaVM : ViewModelServiceBase<Area>
{
    private readonly Dictionary<string, AreaNode> _areaIndex = new();
    private readonly Dictionary<string, GrupoNode> _grupoIndex = new();
    private readonly Dictionary<string, SeccionNode> _seccionIndex = new();
    private readonly Dictionary<string, TurnoNode> _turnoIndex = new();

    private TreeNodeBase? _selectedTreeNode;


    public PageAreaVM()
    {
        PageTurnoVm = new PageTurnoVM();
        PageSeccionVm = new PageSeccionVM();
        PageGrupoVm = new PageGrupoVM();

        ActualizarArbolCommand = new AsyncRelayCommand(LoadTreeAsync);
    }

    public ObservableCollection<TreeNodeBase> TreeNode { get; } = new();

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

    public IAsyncRelayCommand ActualizarArbolCommand { get; protected set; }

    protected async override Task OnActivateAsync()
    {
        PageIndex = 0;
        PageSize = int.MaxValue;
        
        await base.OnActivateAsync();
        
        PageTurnoVm.ServiceTurno.CollectionChanged += OnServiceCollectionChanged;
        PageSeccionVm.ServiceSeccion.CollectionChanged += OnServiceCollectionChanged;
        PageGrupoVm.ServiceGrupo.CollectionChanged += OnServiceCollectionChanged;
        
        await PageTurnoVm.ActivateAsync();
        await PageSeccionVm.ActivateAsync();
        await PageGrupoVm.ActivateAsync();
        
        await ActualizarArbolCommand.ExecuteAsync(null);
    }

    protected async override Task OnDeactivateAsync()
    {
        await base.OnDeactivateAsync();
        await PageTurnoVm.DeactivateAsync();
        await PageSeccionVm.DeactivateAsync();
        await PageGrupoVm.DeactivateAsync();
    }

    protected override async void OnServiceCollectionChanged(EntityChangeType type, string id, Area? entity)
    {
        switch (type)
        {
            case EntityChangeType.Created:
                if (entity != null)
                {
                    var nodoC = CrearNodoArea(entity);
                    TreeNode.Add(nodoC);
                    _areaIndex[nodoC.Id] = nodoC;
                }
                break;

            case EntityChangeType.Updated:
                if (entity != null &&
                    _areaIndex.TryGetValue(id, out var nodoU))
                {
                    nodoU.Area = entity;
                }
                break;

            case EntityChangeType.Deleted:
                if (_areaIndex.TryGetValue(id, out var nodo))
                {
                    TreeNode.Remove(nodo);
                    _areaIndex.Remove(id);
                }
                break;
        }
    }

    private void OnServiceCollectionChanged(EntityChangeType type, string id, Turno? entity)
    {
        switch (type)
        {
            case EntityChangeType.Created:
                if (entity != null &&
                    _areaIndex.TryGetValue(entity.AreaId, out var parent))
                {
                    var nodoC = CrearNodoTurno(entity, parent);
                    parent.Children.Add(nodoC);
                    _turnoIndex[nodoC.Id] = nodoC;
                }
                break;

            case EntityChangeType.Updated:
                if (entity != null &&
                    _turnoIndex.TryGetValue(id, out var nodoU))
                {
                    nodoU.Turno = entity;
                }
                break;

            case EntityChangeType.Deleted:
                if (_turnoIndex.TryGetValue(id, out var nodo))
                {
                    nodo.Parent?.Children.Remove(nodo);
                    _turnoIndex.Remove(id);
                }
                break;
        }
    }


    private void OnServiceCollectionChanged(EntityChangeType type, string id, Seccion? entity)
    {
        switch (type)
        {
            case EntityChangeType.Created:
                if (entity != null &&
                    _turnoIndex.TryGetValue(entity.TurnoId, out var parent))
                {
                    var nodo = CrearNodoSeccion(entity, parent);
                    parent.Children.Add(nodo);
                    _seccionIndex[nodo.Id] = nodo;
                }
                break;

            case EntityChangeType.Updated:
                if (entity != null &&
                    _seccionIndex.TryGetValue(id, out var nodoU))
                {
                    nodoU.Seccion = entity;
                }
                break;

            case EntityChangeType.Deleted:
                if (_seccionIndex.TryGetValue(id, out var nodoD))
                {
                    nodoD.Parent?.Children.Remove(nodoD);
                    _seccionIndex.Remove(id);
                }
                break;
        }
    }

    private void OnServiceCollectionChanged(EntityChangeType type, string id, Grupo? entity)
    {
        switch (type)
        {
            case EntityChangeType.Created:
                if (entity != null &&
                    _seccionIndex.TryGetValue(entity.SeccionId, out var parent))
                {
                    var nodo = CrearNodoGrupo(entity, parent);
                    parent.Children.Add(nodo);
                    _grupoIndex[nodo.Id] = nodo;
                }
                break;

            case EntityChangeType.Updated:
                if (entity != null &&
                    _grupoIndex.TryGetValue(id, out var nodoU))
                {
                    nodoU.Grupo = entity;
                }
                break;

            case EntityChangeType.Deleted:
                if (_grupoIndex.TryGetValue(id, out var nodoD))
                {
                    nodoD.Parent?.Children.Remove(nodoD);
                    _grupoIndex.Remove(id);
                }
                break;
        }
    }



    private async Task LoadTreeAsync()
    {
        try
        {
            await CreateTreeNode();
        }
        catch (Exception ex)
        {
            DialogServiceI.MensajeQueue.Enqueue("Error cargando el árbol: " + ex.Message);
        }
    }


    private async Task CreateTreeNode()
    {
        TreeNode.Clear();

        _areaIndex.Clear();
        _turnoIndex.Clear();
        _seccionIndex.Clear();
        _grupoIndex.Clear();

        var areas = ServicioBase.CacheById.Values.ToList();
        var turnos = PageTurnoVm.ServiceTurno.CacheById.Values.ToList();
        var secciones = PageSeccionVm.ServiceSeccion.CacheById.Values.ToList();
        var grupos = PageGrupoVm.ServiceGrupo.CacheById.Values.ToList();

        // 1. Crear nodos de área
        foreach (var area in areas)
        {
            var areaNode = CrearNodoArea(area);
            TreeNode.Add(areaNode);
            _areaIndex[area.Id] = areaNode;
        }

        await Task.Yield();

        // 2. Crear nodos de turno y asignarlos a su área
        foreach (var turno in turnos)
        {
            if (!_areaIndex.TryGetValue(turno.AreaId, out var parent))
                continue;

            var turnoNode = CrearNodoTurno(turno, parent);
            parent.Children.Add(turnoNode);
            _turnoIndex[turno.Id] = turnoNode;
        }

        await Task.Yield();

        // 3. Crear nodos de sección y asignarlos a su turno
        foreach (var seccion in secciones)
        {
            if (!_turnoIndex.TryGetValue(seccion.TurnoId, out var parent))
                continue;

            var seccionNode = CrearNodoSeccion(seccion, parent);
            parent.Children.Add(seccionNode);
            _seccionIndex[seccion.Id] = seccionNode;
        }

        await Task.Yield();

        // 4. Crear nodos de grupo y asignarlos a su sección
        foreach (var grupo in grupos)
        {
            if (!_seccionIndex.TryGetValue(grupo.SeccionId, out var parent))
                continue;

            var grupoNode = CrearNodoGrupo(grupo, parent);
            parent.Children.Add(grupoNode);
            _grupoIndex[grupo.Id] = grupoNode;
        }
    }


    private async Task<List<T>> ObtenerEntidades<T>(IEnumerable<string> ids, ServiceBase<T> service)
        where T : class, IModelObj, new()
    {
        var tasks = ids.Select(id => service.GetByIdAsync(id));
        var results = await Task.WhenAll(tasks);

        return results
            .Where(r => r.Success)
            .Select(r => r.EntityGet)
            .ToList();
    }


    private AreaNode CrearNodoArea(Area area)
    {
        return new AreaNode
        {
            Id = area.Id,
            Area = area,

            AddCommand = PageTurnoVm.CrearEntityCommand,
            EditCommand = EditarEntityCommand,
            DeleteCommand = EliminarEntityCommand
        };
    }

    private TurnoNode CrearNodoTurno(Turno turno, AreaNode parent)
    {
        return new TurnoNode
        {
            Id = turno.Id,
            Turno = turno,
            Parent = parent,

            AddCommand = PageSeccionVm.CrearEntityCommand,
            EditCommand = PageTurnoVm.EditarEntityCommand,
            DeleteCommand = PageTurnoVm.EliminarEntityCommand
        };
    }

    private SeccionNode CrearNodoSeccion(Seccion seccion, TurnoNode parent)
    {
        return new SeccionNode
        {
            Id = seccion.Id,
            Seccion = seccion,
            Parent = parent,

            AddCommand = PageGrupoVm.CrearEntityCommand,
            EditCommand = PageSeccionVm.EditarEntityCommand,
            DeleteCommand = PageSeccionVm.EliminarEntityCommand
        };
    }

    private GrupoNode CrearNodoGrupo(Grupo grupo, SeccionNode parent)
    {
        return new GrupoNode
        {
            Id = grupo.Id,
            Grupo = grupo,
            Parent = parent,

            EditCommand = PageGrupoVm.EditarEntityCommand,
            DeleteCommand = PageGrupoVm.EliminarEntityCommand
        };
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