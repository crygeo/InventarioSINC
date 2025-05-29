<<<<<<< HEAD
﻿using MaterialDesignColors.Recommended;
=======
﻿using Cliente.src.Attributes;
using MaterialDesignColors.Recommended;
>>>>>>> 29/05/2025
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
<<<<<<< HEAD
        private ICalidad _calidad;
        private float _pesoDesecho;
        private float _pesoNeto;

        public IRecepcionCarga RecepcionCarga { get; }
        public ICalidad Calidad { get => _calidad; set => SetProperty(ref _calidad, value); }
        public float PesoNeto { get => _pesoNeto; set { SetProperty(ref _pesoNeto, value); UpdateChanged(); } }
=======
        private string _id;
        private ICalidad _calidad;
        private float _pesoDesecho;
        private float _pesoNeto;
        private bool _deleteable = false;

        public IRecepcionCarga RecepcionCarga { get; }
        public ICalidad Calidad { get => _calidad; set => SetProperty(ref _calidad, value); }
        [Solicitar("Peso Neto")]
        public float PesoNeto { get => _pesoNeto; set { SetProperty(ref _pesoNeto, value); UpdateChanged(); } }
        [Solicitar("Peso Desecho")]
>>>>>>> 29/05/2025
        public float PesoDesecho { get => _pesoDesecho; set { SetProperty(ref _pesoDesecho, value); UpdateChanged(); } }

        public float PesoBruto { get => PesoNeto - PesoDesecho; }
        public float PesoBrutoProcesado { get => CochesList.Sum(a => a.PesoBruto); }
<<<<<<< HEAD
        public float Rendimiento { get => (PesoBrutoProcesado/ PesoBruto) * 100; }
=======
        public float Rendimiento { get => (PesoBrutoProcesado / PesoBruto) * 100; }
>>>>>>> 29/05/2025


        public IReadOnlyList<ICoche> CochesList => throw new NotImplementedException();

<<<<<<< HEAD
        public override string Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void AgregarProducto(IEmpaqueUnidad empaqueUnidad)
        {
            throw new NotImplementedException();
=======
        public override string Id { get => _id; set => SetProperty(ref _id, value); }
        public bool Deleteable { get => _deleteable; set => SetProperty(ref _deleteable, value); }
        public void AgregarProducto(IEmpaqueUnidad empaqueUnidad)
        {
            
>>>>>>> 29/05/2025
        }

        public void QuitarProducto(IEmpaqueUnidad empaqueUnidad)
        {
<<<<<<< HEAD
            throw new NotImplementedException();
=======
>>>>>>> 29/05/2025
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
