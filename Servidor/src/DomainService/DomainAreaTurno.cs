using System.Linq;
using System.Threading.Tasks;
using Servidor.Model;
using Servidor.Services;
using Shared.Attributes;

namespace Servidor.DomainService;

[AutoService]
public class DomainAreaTurno
{
    private readonly ServiceBase<Area> _serviceArea;
    private readonly ServiceBase<Turno> _serviceTurno;
    private readonly ServiceBase<Seccion> _serviceSeccion;
    private  readonly ServiceBase<Grupo> _serviceGrupo;
    

    public DomainAreaTurno()
    {
        _serviceArea = ServiceFactory.GetService<Area>();
        _serviceTurno = ServiceFactory.GetService<Turno>();
        _serviceSeccion = ServiceFactory.GetService<Seccion>();
        _serviceGrupo = ServiceFactory.GetService<Grupo>();
    }

    /// <summary>
    /// Elimina un Área junto con toda su jerarquía de entidades relacionadas,
    /// incluyendo Turnos, Secciones y Grupos asociados.
    /// 
    /// El proceso se realiza de manera descendente:
    /// 1. Se valida la existencia del Área.
    /// 2. Se obtienen los Turnos pertenecientes al Área.
    /// 3. Se obtienen las Secciones asociadas a dichos Turnos.
    /// 4. Se eliminan en bloque los Grupos vinculados a las Secciones.
    /// 5. Se eliminan las Secciones.
    /// 6. Se eliminan los Turnos.
    /// 7. Finalmente, se elimina el Área.
    /// 
    /// Esto garantiza que no queden documentos huérfanos en la base de datos
    /// y mantiene la integridad lógica del modelo jerárquico.
    /// </summary>
    /// <param name="id">
    /// Identificador del Área a eliminar.
    /// </param>
    /// <returns>
    /// <c>true</c> si el Área existía y fue eliminada correctamente junto con
    /// sus entidades relacionadas; de lo contrario, <c>false</c>.
    /// </returns>
    public async Task<bool> EliminarAreaHeHijosAsync(string id)
    {
        var area = await _serviceArea.GetByIdAsync(id);
        if (area == null)
            return false;

        // 1️⃣ Obtener IDs de turnos
        var turnos = await _serviceTurno.GetItemsAsync(x => x.AreaId == id);
        var turnoIds = turnos.Select(t => t.Id).ToList();

        // 2️⃣ Obtener IDs de secciones
        var secciones = await _serviceSeccion.GetItemsAsync(x => turnoIds.Contains(x.TurnoId));
        var seccionIds = secciones.Select(s => s.Id).ToList();

        // 3️⃣ Eliminar grupos en bloque
        await _serviceGrupo.RemoveItemsAsync(x => seccionIds.Contains(x.SeccionId));

        // 4️⃣ Eliminar secciones en bloque
        await _serviceSeccion.RemoveItemsAsync(x => turnoIds.Contains(x.TurnoId));

        // 5️⃣ Eliminar turnos en bloque
        await _serviceTurno.RemoveItemsAsync(x => x.AreaId == id);

        // 6️⃣ Eliminar área
        await _serviceArea.DeleteAsync(id);

        return true;
    }
}

[AutoService]
public class DomainTurnoSeccion
{
    private readonly ServiceBase<Turno> _serviceTurno;
    private readonly ServiceBase<Seccion> _serviceSeccion;
    private readonly  ServiceBase<Grupo> _serviceGrupo;

    public DomainTurnoSeccion()
    {
        _serviceTurno = ServiceFactory.GetService<Turno>();
        _serviceSeccion = ServiceFactory.GetService<Seccion>();
            _serviceGrupo = ServiceFactory.GetService<Grupo>();
    }

    /// <summary>
    /// Elimina un Turno junto con todas las entidades relacionadas en su jerarquía,
    /// incluyendo las Secciones y los Grupos asociados.
    /// 
    /// El proceso se ejecuta de forma descendente:
    /// 1. Se valida la existencia del Turno.
    /// 2. Se obtienen las Secciones pertenecientes al Turno.
    /// 3. Se eliminan en bloque los Grupos vinculados a dichas Secciones.
    /// 4. Se eliminan las Secciones.
    /// 5. Finalmente, se elimina el Turno.
    /// 
    /// Esto asegura que no queden documentos huérfanos y mantiene
    /// la consistencia lógica del modelo jerárquico.
    /// </summary>
    /// <param name="id">
    /// Identificador del Turno a eliminar.
    /// </param>
    /// <returns>
    /// <c>true</c> si el Turno existía y fue eliminado correctamente junto con
    /// sus entidades relacionadas; en caso contrario, <c>false</c>.
    /// </returns>
    public async Task<bool> EliminarTurnoHeHijosAsync(string id)
    {
        var turno = await _serviceTurno.GetByIdAsync(id);
        if (turno == null)
            return false;

        // 1️⃣ Obtener IDs de secciones del turno
        var secciones = await _serviceSeccion.GetItemsAsync(x => x.TurnoId == id);
        var seccionIds = secciones.Select(s => s.Id).ToList();

        // 2️⃣ Eliminar grupos asociados a esas secciones
        await _serviceGrupo.RemoveItemsAsync(x => seccionIds.Contains(x.SeccionId));

        // 3️⃣ Eliminar secciones
        await _serviceSeccion.RemoveItemsAsync(x => x.TurnoId == id);

        // 4️⃣ Eliminar turno
        await _serviceTurno.DeleteAsync(id);

        return true;
    }
}
[AutoService]
public class DomainSeccionGrupo
{
    private readonly ServiceBase<Seccion> _serviceSeccion;
    private readonly ServiceBase<Grupo> _serviceGrupo;

    public DomainSeccionGrupo()
    {
        _serviceSeccion = ServiceFactory.GetService<Seccion>();
        _serviceGrupo = ServiceFactory.GetService<Grupo>();
    }

    /// <summary>
    /// Elimina una Sección junto con todas las entidades relacionadas en su jerarquía,
    /// específicamente los Grupos asociados.
    /// 
    /// El proceso se ejecuta de forma descendente:
    /// 1. Se valida la existencia de la Sección.
    /// 2. Se eliminan en bloque los Grupos vinculados a la Sección.
    /// 3. Finalmente, se elimina la Sección.
    /// 
    /// Esto garantiza que no queden documentos huérfanos y mantiene
    /// la consistencia lógica del modelo jerárquico.
    /// </summary>
    /// <param name="id">
    /// Identificador de la Sección a eliminar.
    /// </param>
    /// <returns>
    /// <c>true</c> si la Sección existía y fue eliminada correctamente junto con
    /// sus entidades relacionadas; de lo contrario, <c>false</c>.
    /// </returns>
    public async Task<bool> EliminarSeccionHeHijosAsync(string id)
    {
        var seccion = await _serviceSeccion.GetByIdAsync(id);
        if (seccion == null)
            return false;

        // 1️⃣ Eliminar grupos asociados a la sección
        await _serviceGrupo.RemoveItemsAsync(x => x.SeccionId == id);

        // 2️⃣ Eliminar sección
        await _serviceSeccion.DeleteAsync(id);

        return true;
    }
}