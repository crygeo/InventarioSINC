// Cliente/src/ViewModel/Model/PageAreaVM.cs
using System.Collections.ObjectModel;
using Cliente.Helpers;
using Cliente.Messages;
using Cliente.Obj;
using Cliente.Obj.Model;
using Cliente.Services.Model;
using Cliente.ViewModel.Model.Detail;
using Cliente.ViewModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Shared.Interfaces.Model;
using Utilidades.Mvvm;

namespace Cliente.ViewModel.Model;

public partial class PageAreaVM : ViewModelServiceBase<Area>
{
    // ── Árbol plano (izquierda) ──────────────────────────────────────────
    public ObservableCollection<AreaNode> AreaNodes { get; } = [];

    private AreaNode? _selectedAreaNode;
    public AreaNode? SelectedAreaNode
    {
        get => _selectedAreaNode;
        set
        {
            if (!SetProperty(ref _selectedAreaNode, value)) return;
            if (value != null)
                NavigateToArea(value.Area);
        }
    }

    // ── Navegación derecha ───────────────────────────────────────────────
    public NavigationStack DetailNavigation { get; } = new();

    // ── Hijos (se activan junto con el padre) ────────────────────────────
    public PageTurnoVM PageTurnoVm { get; }
    public PageSeccionVM PageSeccionVm { get; }
    public PageGrupoVM PageGrupoVm { get; }

    public IAsyncRelayCommand RefreshCommand { get; }

    public PageAreaVM()
    {
        PageTurnoVm  = new PageTurnoVM();
        PageSeccionVm = new PageSeccionVM();
        PageGrupoVm  = new PageGrupoVM();

        RefreshCommand = new AsyncRelayCommand(LoadTreeAsync);
        
        
    }

    // ── Ciclo de vida ────────────────────────────────────────────────────

    protected override async Task OnActivateAsync()
    {
        PageIndex = 0;
        PageSize  = int.MaxValue;

        await base.OnActivateAsync();

        // Suscribir hijos
        PageTurnoVm.ServiceTurno.CollectionChanged   += OnTurnoChanged;
        PageSeccionVm.ServiceSeccion.CollectionChanged += OnSeccionChanged;
        PageGrupoVm.ServiceGrupo.CollectionChanged   += OnGrupoChanged;

        await PageTurnoVm.ActivateAsync();
        await PageSeccionVm.ActivateAsync();
        await PageGrupoVm.ActivateAsync();

        WeakReferenceMessenger.Default.Register<NavigateToDetailMessage>(this, (r, m) =>
        {
            DetailNavigation.Push(m.Value);
        });
        
        await LoadTreeAsync();
    }

    protected override async Task OnDeactivateAsync()
    {
        PageTurnoVm.ServiceTurno.CollectionChanged   -= OnTurnoChanged;
        PageSeccionVm.ServiceSeccion.CollectionChanged -= OnSeccionChanged;
        PageGrupoVm.ServiceGrupo.CollectionChanged   -= OnGrupoChanged;

        await PageTurnoVm.DeactivateAsync();
        await PageSeccionVm.DeactivateAsync();
        await PageGrupoVm.DeactivateAsync();

        WeakReferenceMessenger.Default.UnregisterAll(this);
        
        await base.OnDeactivateAsync();
    }

    // ── Carga del árbol ──────────────────────────────────────────────────

    private async Task LoadTreeAsync()
    {
        AreaNodes.Clear();
        DetailNavigation.Reset(new EmptyDetailVM());

        var areas    = ServicioBase.CacheById.Values.ToList();
        var turnos   = PageTurnoVm.ServiceTurno.CacheById.Values.ToList();
        var secciones = PageSeccionVm.ServiceSeccion.CacheById.Values.ToList();
        var grupos   = PageGrupoVm.ServiceGrupo.CacheById.Values.ToList();

        foreach (var area in areas)
        {
            var node = new AreaNode
            {
                Id   = area.Id,
                Area = area,
                AddCommand    = PageTurnoVm.CrearEntityFromItemCommand,
                EditCommand   = EditarEntityFromItemCommand,
                DeleteCommand = EliminarEntityFromItemCommand
            };

            // Turnos del área
            foreach (var turno in turnos.Where(t => t.AreaId == area.Id))
            {
                var turnoNode = new TurnoNode
                {
                    Id     = turno.Id,
                    Turno  = turno,
                    Parent = node,
                    AddCommand    = PageSeccionVm.CrearEntityFromItemCommand,
                    EditCommand   = PageTurnoVm.EditarEntityFromItemCommand,
                    DeleteCommand = PageTurnoVm.EliminarEntityFromItemCommand
                };

                foreach (var seccion in secciones.Where(s => s.TurnoId == turno.Id))
                {
                    var seccionNode = new SeccionNode
                    {
                        Id      = seccion.Id,
                        Seccion = seccion,
                        Parent  = turnoNode,
                        AddCommand    = PageGrupoVm.CrearEntityFromItemCommand,
                        EditCommand   = PageSeccionVm.EditarEntityFromItemCommand,
                        DeleteCommand = PageSeccionVm.EliminarEntityFromItemCommand
                    };

                    foreach (var grupo in grupos.Where(g => g.SeccionId == seccion.Id))
                    {
                        seccionNode.Children.Add(new GrupoNode
                        {
                            Id     = grupo.Id,
                            Grupo  = grupo,
                            Parent = seccionNode,
                            EditCommand   = PageGrupoVm.EditarEntityFromItemCommand,
                            DeleteCommand = PageGrupoVm.EliminarEntityFromItemCommand
                        });
                    }

                    turnoNode.Children.Add(seccionNode);
                }

                node.Children.Add(turnoNode);
            }

            AreaNodes.Add(node);
        }
    }

    // ── Navegación al detalle ─────────────────────────────────────────────

    /// <summary>
    /// Construye el ViewModel de detalle y lo pushea al stack.
    /// Toda la construcción está en la factory — no inline.
    /// </summary>
    private void NavigateToArea(Area area)
    {
        var detailVm = DetailViewModelFactory.BuildAreaDetail(
            area,
            PageTurnoVm.ServiceTurno.CacheById.Values,
            PageSeccionVm.ServiceSeccion.CacheById.Values,
            PageGrupoVm.ServiceGrupo.CacheById.Values);

        DetailNavigation.Reset(detailVm);
        SetChildContexts(null, null, null); // limpia contextos hijos
        EntitySelect = area;
    }

    // ── Reacciones a cambios en SignalR ──────────────────────────────────

    protected override void OnServiceCollectionChanged(EntityChangeType type, string id, Area? entity)
    {
        // Actualizamos el nodo en el árbol sin recargar todo
        switch (type)
        {
            case EntityChangeType.Created when entity != null:
                var newNode = new AreaNode { Id = entity.Id, Area = entity,
                    AddCommand = PageTurnoVm.CrearEntityFromItemCommand,
                    EditCommand = EditarEntityFromItemCommand,
                    DeleteCommand = EliminarEntityFromItemCommand };
                AreaNodes.Add(newNode);
                break;

            case EntityChangeType.Updated when entity != null:
                var existing = AreaNodes.FirstOrDefault(n => n.Id == id);
                if (existing != null) existing.Area = entity;
                break;

            case EntityChangeType.Deleted:
                var toRemove = AreaNodes.FirstOrDefault(n => n.Id == id);
                if (toRemove != null) AreaNodes.Remove(toRemove);
                break;
        }
    }

    private void OnTurnoChanged(EntityChangeType type, string id, Turno? entity)
        => _ = LoadTreeAsync(); // reconstruye — los turnos son pocos

    private void OnSeccionChanged(EntityChangeType type, string id, Seccion? entity)
        => _ = LoadTreeAsync();

    private void OnGrupoChanged(EntityChangeType type, string id, Grupo? entity)
        => _ = LoadTreeAsync();

    private void SetChildContexts(Area? area, Turno? turno, Seccion? seccion)
    {
        PageTurnoVm.AreaPadre   = area;
        PageSeccionVm.TurnoPadre = turno;
        PageGrupoVm.SeccionPadre = seccion;
    }

    protected override void UpdateChanged() => base.UpdateChanged();
}

/// <summary>Placeholder cuando no hay selección activa.</summary>
public class EmptyDetailVM : ViewModelBase
{
    protected override void UpdateChanged() { }
}