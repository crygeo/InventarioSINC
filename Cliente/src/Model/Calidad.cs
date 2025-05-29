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
<<<<<<< HEAD
=======
    [Obsolete("Esta clase está deshabilitada. No debe usarse.", true)]

    [NombreEntidad("Color", "Colores")]
>>>>>>> 29/05/2025
    public class Calidad : ModelBase<ICalidad>, ICalidad
    {
        private string _id = string.Empty;
        private string _nombre = string.Empty;
        private string _descripcion = string.Empty;
<<<<<<< HEAD

        public override string Id { get => _id; set => _id = value; }
        public string Nombre { get => _nombre; set => _nombre = value; }
        public string Descripcion { get => _descripcion; set => _descripcion = value; }

=======
        private bool _deletable = true;

        public override string Id { get => _id; set => _id = value; }

        [Solicitar("Nombre")]
        public string Nombre { get => _nombre; set => _nombre = value; }

        [Solicitar("Descripcion")]
        public string Descripcion { get => _descripcion; set => _descripcion = value; }

        public bool Deleteable { get; set; }

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
