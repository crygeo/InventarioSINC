using Shared.Interfaces;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades.Interfaces;

namespace Cliente.src.Model
{
    public class Proveedor : ModelBase<IProveedor>, IProveedor
    {
        private string _id = string.Empty;
        private string _ruc = string.Empty;
        private string _direccion = string.Empty;

        public override string Id { get => _id; set => SetProperty(ref _id, value); }
        public string RUC { get => _ruc; set => SetProperty(ref _ruc, value); }
        public string Direccion { get => _direccion; set => SetProperty(ref _direccion, value); }

        public override void Update(IIdentifiable identity)
        {
            if(identity is not IProveedor proveedor)
                throw new ArgumentException("El objeto no es un proveedor válido.");
            Id = proveedor.Id;
            RUC = proveedor.RUC;
            Direccion = proveedor.Direccion;
        }

        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }
}
