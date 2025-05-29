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
<<<<<<< HEAD
        public ProveedorService() : base(new ProveedorHubService()) { }
=======
        private ProveedorService() : base(new ProveedorHubService()) { }
>>>>>>> 29/05/2025
    }

    public class TallaService : ServiceBase<Talla>
    {
        private static readonly Lazy<TallaService> _instance = new(() => new TallaService());
        public static TallaService Instance { get => _instance.Value; }
<<<<<<< HEAD
        public TallaService() : base(new TallaHubService()) { }
=======
        private TallaService() : base(new TallaHubService()) { }
>>>>>>> 29/05/2025
    }

    public class RecepcionCargaService : ServiceBase<RecepcionCarga>
    {
        private static readonly Lazy<RecepcionCargaService> _instance = new(() => new RecepcionCargaService());
        public static RecepcionCargaService Instance { get => _instance.Value; }
<<<<<<< HEAD
        public RecepcionCargaService() : base(new RecepcionCargaHubService()) { }
=======
        private RecepcionCargaService() : base(new RecepcionCargaHubService()) { }
>>>>>>> 29/05/2025
    }

    public class ClasificacionService : ServiceBase<Clasificacion>
    {
        private static readonly Lazy<ClasificacionService> _instance = new(() => new ClasificacionService());
        public static ClasificacionService Instance { get => _instance.Value; }
<<<<<<< HEAD
        public ClasificacionService() : base(new ClasificacionHubService()) { }
=======
        private ClasificacionService() : base(new ClasificacionHubService()) { }
>>>>>>> 29/05/2025
    }

    public class ClaseService : ServiceBase<Clase>
    {
        private static readonly Lazy<ClaseService> _instance = new(() => new ClaseService());
        public static ClaseService Instance { get => _instance.Value; }
<<<<<<< HEAD
        public ClaseService() : base(new ClaseHubService()) { }
=======
        private ClaseService() : base(new ClaseHubService()) { }
>>>>>>> 29/05/2025
    }

    public class CalidadService : ServiceBase<Calidad>
    {
        private static readonly Lazy<CalidadService> _instance = new(() => new CalidadService());
        public static CalidadService Instance { get => _instance.Value; }
<<<<<<< HEAD
        public CalidadService() : base(new CalidadHubService()) { }
=======
        private CalidadService() : base(new CalidadHubService()) { }
>>>>>>> 29/05/2025
    }
}
