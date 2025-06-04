using System;
using System.Collections.Generic;
using Shared.Interfaces;
using Shared.Interfaces.ModelsBase;

namespace Servidor.src.Objs
{
    public class RecepcionCarga : IRecepcionCarga
    {
        public string Id { get; set; } = String.Empty;
        public bool Deleteable { get; set; } = true;
        public IProveedor Proveedor { get; set; } = new ProveedorPersona();
        public DateTime FechaIngreso { get; set; }
        public List<ICarga> Camiones { get; set; } = [];
        public float PesoTotal { get; set; }
        public byte[]? GuiaGlobal { get; set; }
        public string Nota { get; set; } = string.Empty;
        public IEnumerable<IIdentificador> Identificadores { get; set; } = [];
        public bool Updatable { get; set; }
        public void Update(IModelObj entity)
        {
            throw new NotImplementedException();
        }
        public void CerrarEntrega()
        {
            throw new NotImplementedException();
        }
    }
}
