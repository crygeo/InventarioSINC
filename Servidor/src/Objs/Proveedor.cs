using Shared.Interfaces.ModelsBase;
using System;

namespace Servidor.src.Objs
{
    public class ProveedorEmpresa : IProveedor, IEmpresa
    {
        public string RUC { get; set; }
        public string Direccion { get; set; }
        public string Id { get; set; }
        public string RazonSocial { get; set; }
        public string RepresentanteLegal { get; set; }
        public bool Deleteable { get; set; }
    }

    public class ProveedorPersona : IProveedor, IPersona
    {
        public string RUC { get; set; }
        public string Direccion { get; set; }
        public string Id { get; set; }

        public string PrimerNombre { get;}

        public string SegundoNombre { get; }

        public string PrimerApellido { get; }

        public string SegundoApellido { get; }

        public string Cedula { get; }
        public string Celular { get; }

        public DateTime FechaNacimiento { get; }

        public bool Deleteable { get; set; }
    }
}
