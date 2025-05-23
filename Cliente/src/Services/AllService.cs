using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cliente.src.Model;
using Cliente.src.ServicesHub;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using Shared.Interfaces.ModelsBase;

namespace Cliente.src.Services
{
    public class ProveedorService : ServiceBase<Proveedor>
    {
        private static readonly Lazy<ProveedorService> _instance = new(() => new ProveedorService());
        public static ProveedorService Instance { get => _instance.Value; }
        public ProveedorService() : base(new ProveedorHubService()) { }
    }

    public class TallaService : ServiceBase<Talla>
    {
        private static readonly Lazy<TallaService> _instance = new(() => new TallaService());
        public static TallaService Instance { get => _instance.Value; }
        public TallaService() : base(new TallaHubService()) { }
    }

    public class RecepcionCargaService : ServiceBase<RecepcionCarga>
    {
        private static readonly Lazy<RecepcionCargaService> _instance = new(() => new RecepcionCargaService());
        public static RecepcionCargaService Instance { get => _instance.Value; }
        public RecepcionCargaService() : base(new RecepcionCargaHubService()) { }
    }

    public class ClasificacionService : ServiceBase<Clasificacion>
    {
        private static readonly Lazy<ClasificacionService> _instance = new(() => new ClasificacionService());
        public static ClasificacionService Instance { get => _instance.Value; }
        public ClasificacionService() : base(new ClasificacionHubService()) { }
    }

    public class ClaseService : ServiceBase<Clase>
    {
        private static readonly Lazy<ClaseService> _instance = new(() => new ClaseService());
        public static ClaseService Instance { get => _instance.Value; }
        public ClaseService() : base(new ClaseHubService()) { }
    }

    public class CalidadService : ServiceBase<Calidad>
    {
        private static readonly Lazy<CalidadService> _instance = new(() => new CalidadService());
        public static CalidadService Instance { get => _instance.Value; }
        public CalidadService() : base(new CalidadHubService()) { }
    }
}
