using Cliente.src.Model;
using Cliente.src.Services;
using Shared.Interfaces;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.src.ViewModel
{
    public class PageProveedor : ViewModelServiceBase<Proveedor>
    {
        public override ServiceBase<Proveedor> ServicioBase => new ProveedorService();

        public override Task CrearEntityAsync()
        {
            throw new NotImplementedException();
        }

        public override Task DeleteEntityAsync()
        {
            throw new NotImplementedException();
        }

        public override Task EditarEntityAsync()
        {
            throw new NotImplementedException();
        }

        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }
}
