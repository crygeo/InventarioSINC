using Cliente.src.Attributes;
using MaterialDesignColors.Recommended;
using Shared.Interfaces;
using Shared.Interfaces.Model.Obj;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Utilidades.Controls;

namespace Cliente.src.Model
{
    public class Clasificacion : ModelBase<IClasificacion>, IClasificacion
    {
        private float _pesoDesecho;
        private float _pesoNeto;
        private IRecepcionCarga _recepcionCarga = new RecepcionCarga();
        private IReadOnlyList<ICoche> _cochesList = [];


        public IRecepcionCarga RecepcionCarga
        {
            get => _recepcionCarga;
            set => SetProperty(ref _recepcionCarga, value);
        }
        [Solicitar("Peso Neto", ItemType = typeof(TextBox), InputBoxConvert = InputBoxType.Decimal)]
        public float PesoNeto { get => _pesoNeto; set { SetProperty(ref _pesoNeto, value); UpdateChanged(); } }
        [Solicitar("Peso Desecho", ItemType = typeof(TextBox), InputBoxConvert = InputBoxType.Decimal)]
        public float PesoDesecho { get => _pesoDesecho; set { SetProperty(ref _pesoDesecho, value); UpdateChanged(); } }

        public float PesoBruto { get => PesoNeto - PesoDesecho;}
        public float PesoBrutoProcesado { get => CochesList.Sum(a => a.PesoBruto); }
        public float Rendimiento { get => (PesoBrutoProcesado / PesoBruto) * 100; }

        public IReadOnlyList<ICoche> CochesList
        {
            get => _cochesList;
            set => _cochesList = value;
        }


        public void AgregarProducto(IEmpaqueUnidad empaqueUnidad)
        {
            
        }

        public void QuitarProducto(IEmpaqueUnidad empaqueUnidad)
        {
        }

        public override void Update(IModelObj identity)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateChanged()
        {
            OnPropertyChanged(nameof(PesoBruto));
            OnPropertyChanged(nameof(Rendimiento));
        }
    }
}
