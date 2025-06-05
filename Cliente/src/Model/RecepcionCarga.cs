using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cliente.src.Attributes;
using Cliente.src.View.Items;
using Utilidades.Controls;
using Shared.Interfaces.Model;
using Shared.Interfaces.Model.Obj;

namespace Cliente.src.Model
{
    [NombreEntidad("Recepcion")]
    public class RecepcionCarga : ModelBase<IRecepcionCarga>, IRecepcionCarga
    {
        private IEnumerable<Identificador> _identificadores = new ObservableCollection<Identificador>();

        private IProveedor _proveedor = new ProveedorPersona();
        private DateTime _fechaIngreso = DateTime.Now;
        private List<ICarga> _camiones = [];
        private float _pesoTotal;
        private byte[]? _guiaGlobal;
        private string _nota = string.Empty;

        [Solicitar("Identificadores", ItemType = typeof(IdentificadoresSelect), InputBoxConvert = InputBoxType.None)]
        public IEnumerable<Identificador> Identificadores
        {
            get => _identificadores;
            set => SetProperty(ref _identificadores, value);
        }
        IEnumerable<IIdentificador> IRecepcionCarga.Identificadores
        {
            get => Identificadores;
            set => Identificadores = value.Cast<Identificador>();
        }


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
        public override void Update(IModelObj identity)
        {
            base.Update(identity);
            if (identity is IRecepcionCarga recepcionCarga)
            {
                Proveedor = recepcionCarga.Proveedor;
                FechaIngreso = recepcionCarga.FechaIngreso;
                Camiones = recepcionCarga.Camiones;
                PesoTotal = recepcionCarga.PesoTotal;
                GuiaGlobal = recepcionCarga.GuiaGlobal;
                Nota = recepcionCarga.Nota;
            }
        }
        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }
}
