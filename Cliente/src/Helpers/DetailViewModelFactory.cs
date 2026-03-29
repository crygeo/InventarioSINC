using Cliente.Obj.Model;
using Cliente.ViewModel.Model.Detail;

namespace Cliente.Helpers;

/// <summary>
/// Construye ViewModels de detalle a partir del árbol de nodos.
/// Centraliza la lógica de ensamblado para que PageAreaVM no lo haga inline.
/// </summary>
public static class DetailViewModelFactory
{
    public static AreaDetailVM BuildAreaDetail(
        Area area,
        IEnumerable<Turno> turnos,
        IEnumerable<Seccion> secciones,
        IEnumerable<Grupo> grupos)
    {
        var turnoVMs = turnos
            .Where(t => t.AreaId == area.Id)
            .Select(t => BuildTurnoDetail(t, area, secciones, grupos))
            .ToList();

        return new AreaDetailVM(area, turnoVMs);
    }

    public static TurnoDetailVM BuildTurnoDetail(
        Turno turno,
        Area areaPadre,
        IEnumerable<Seccion> secciones,
        IEnumerable<Grupo> grupos)
    {
        var seccionVMs = secciones
            .Where(s => s.TurnoId == turno.Id)
            .Select(s => BuildSeccionDetail(s, turno, grupos))
            .ToList();

        return new TurnoDetailVM(turno, areaPadre, seccionVMs);
    }

    public static SeccionDetailVM BuildSeccionDetail(
        Seccion seccion,
        Turno turnoPadre,
        IEnumerable<Grupo> grupos)
    {
        var grupoVMs = grupos
            .Where(g => g.SeccionId == seccion.Id)
            .Select(g => new GrupoDetailVM(g, seccion))
            .ToList();

        return new SeccionDetailVM(seccion, turnoPadre, grupoVMs);
    }
}