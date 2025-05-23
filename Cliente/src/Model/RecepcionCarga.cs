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
        private IProveedor _proveedor;
        private DateTime _fechaIngreso = DateTime.Now;
        private List<ICarga> _camiones = [];
        private float _pesoTotal;
        private byte[]? _guiaGlobal;
        private string _nota = string.Empty;
        private string _id = string.Empty;
        private string _macroLote = string.Empty;
        private string _lote = string.Empty;
        private bool _entregaCerrada = false;


        public IProveedor Proveedor
        {
            get => _proveedor;
            set => SetProperty(ref _proveedor, value);
        }

        public DateTime FechaIngreso
        {
            get => _fechaIngreso;
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

        public override string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public string MacroLote
        {
            get => _macroLote;
            set => SetProperty(ref _macroLote, value);
        }

        public string Lote
        {
            get => _lote;
            set => SetProperty(ref _lote, value);
        }
        public bool EntregaCerrada
        {
            get => _entregaCerrada;
            set => SetProperty(ref _entregaCerrada, value);
        }

        public void CerrarEntrega()
        {
            EntregaCerrada = true;
        }
        public override void Update(IIdentifiable identity)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }
}
