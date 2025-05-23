using MaterialDesignColors.Recommended;
using Shared.Interfaces;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.src.Model
{
    public class Clasificacion : ModelBase<IClasificacion>, IClasificacion
    {
        private ICalidad _calidad;
        private float _pesoDesecho;
        private float _pesoNeto;

        public IRecepcionCarga RecepcionCarga { get; }
        public ICalidad Calidad { get => _calidad; set => SetProperty(ref _calidad, value); }
        public float PesoNeto { get => _pesoNeto; set { SetProperty(ref _pesoNeto, value); UpdateChanged(); } }
        public float PesoDesecho { get => _pesoDesecho; set { SetProperty(ref _pesoDesecho, value); UpdateChanged(); } }

        public float PesoBruto { get => PesoNeto - PesoDesecho; }
        public float PesoBrutoProcesado { get => CochesList.Sum(a => a.PesoBruto); }
        public float Rendimiento { get => (PesoBrutoProcesado/ PesoBruto) * 100; }


        public IReadOnlyList<ICoche> CochesList => throw new NotImplementedException();

        public override string Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void AgregarProducto(IEmpaqueUnidad empaqueUnidad)
        {
            throw new NotImplementedException();
        }

        public void QuitarProducto(IEmpaqueUnidad empaqueUnidad)
        {
            throw new NotImplementedException();
        }

        public override void Update(IIdentifiable identity)
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
