using Shared.Interfaces.ModelsBase;
using System;

namespace Servidor.src.Objs
{
    public class ProveedorEmpresa : IProveedorEmpresa
    {
        public string RUC { get; set; }
        public string Direccion { get; set; }
        public string Id { get; set; }
        public string RazonSocial { get; set; }
        public string RepresentanteLegal { get; set; }
        public bool Deleteable { get; set; }
        public bool Updatable { get; set; }
        public void Update(IModelObj entity)
        {
            throw new NotImplementedException();
        }
    }

    public class ProveedorPersona : IProveedorPersona
    {
        public string RUC { get; set; }
        public string Direccion { get; set; }
        public string Id { get; set; }

        public string PrimerNombre { get; set; }

        public string SegundoNombre { get; set; }

        public string PrimerApellido { get; set; }

        public string SegundoApellido { get; set; }

        public string Cedula { get; set; }
        public string Celular { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public bool Deleteable { get; set; }
        public bool Updatable { get; set; }
        public void Update(IModelObj entity)
        {
            throw new NotImplementedException();
        }
    }
}
