using Shared.Interfaces;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.src.Model
{
    public class Calidad : ModelBase<ICalidad>, ICalidad
    {
        private string _id = string.Empty;
        private string _nombre = string.Empty;
        private string _descripcion = string.Empty;

        public override string Id { get => _id; set => _id = value; }
        public string Nombre { get => _nombre; set => _nombre = value; }
        public string Descripcion { get => _descripcion; set => _descripcion = value; }

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
