using Shared.Interfaces;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.src.Model
{
    public class RecepcionCarga : ModelBase<IRecepcionCarga>, IRecepcionCarga
    {
        private IEnumerable<IIdentificador> _identificadores = [];
        private IProveedor _proveedor = new ProveedorPersona();
        private DateTime _fechaIngreso = DateTime.Now;
        private List<ICarga> _camiones = [];
        private float _pesoTotal;
        private byte[]? _guiaGlobal;
        private string _nota = string.Empty;

        public override void Update(IModelObj identity)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IIdentificador> Identificadores => _identificadores;

        public IProveedor Proveedor
        {
            get => _proveedor;
            set => SetProperty(ref _proveedor, value);
        }

        public DateTime FechaIngreso
        {
            get => _fechaIngreso;
            set => SetProperty(ref _fechaIngreso, value);
        }

        public List<ICarga> Camiones
        {
            get => _camiones;
            set => SetProperty(ref _camiones, value);
        }

        public float PesoTotal
        {
            get => _pesoTotal;
            set => SetProperty(ref _pesoTotal, value);
        }

        public byte[]? GuiaGlobal
        {
            get => _guiaGlobal;
            set => SetProperty(ref _guiaGlobal, value);
        }

        public string Nota
        {
            get => _nota;
            set => SetProperty(ref _nota, value);
        }

        public void CerrarEntrega()
        {
            throw new NotImplementedException();
        }
    }
}
