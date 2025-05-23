using Cliente.src.Model;
using Cliente.src.Services;
using Shared.Interfaces.ModelsBase;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Cliente.src.ServicesHub
{
    public class ProveedorHubService : HubServiceBase<Proveedor>
    {
        public override ObservableCollection<Proveedor> Collection { get; } = new();
    }

    public class TallaHubService : HubServiceBase<Talla>
    {
        public override ObservableCollection<Talla> Collection { get; } = new();
    }

    public class RecepcionCargaHubService : HubServiceBase<RecepcionCarga>
    {
        public override ObservableCollection<RecepcionCarga> Collection { get; } = new();
    }
    public class ClasificacionHubService : HubServiceBase<Clasificacion>
    {
        public override ObservableCollection<Clasificacion> Collection { get; } = new();
    }

    public class ClaseHubService : HubServiceBase<Clase>
    {
        public override ObservableCollection<Clase> Collection { get; } = new();
    }

    public class CalidadHubService : HubServiceBase<Calidad>
    {
        public override ObservableCollection<Calidad> Collection { get; } = new();
    }


}
