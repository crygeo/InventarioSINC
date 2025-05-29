<<<<<<< HEAD
﻿using Shared.Interfaces;
=======
﻿using Cliente.src.Attributes;
using Shared.Interfaces;
>>>>>>> 29/05/2025
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.src.Model
{
    public class Clase : ModelBase<IClase>, IClase
    {
        private string _id = string.Empty;
        private string _nombre = string.Empty;
        private string _descripcion = string.Empty;
<<<<<<< HEAD

        public override string Id { get => _id; set => SetProperty(ref _id, value); }
        public string Name { get => _nombre; set => SetProperty(ref _nombre, value); }
        public string Description { get => _descripcion; set => SetProperty(ref _descripcion, value); }
=======
        private bool _deleteable = false;

        public override string Id { get => _id; set => SetProperty(ref _id, value); }

        [Solicitar("Nombre")]
        public string Name { get => _nombre; set => SetProperty(ref _nombre, value); }
        [Solicitar("Descripcion")]
        public string Description { get => _descripcion; set => SetProperty(ref _descripcion, value); }
        public bool Deleteable { get => _deleteable; set => SetProperty(ref _deleteable, value); }
>>>>>>> 29/05/2025

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
