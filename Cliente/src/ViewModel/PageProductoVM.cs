using Cliente.src.Model;
using Cliente.src.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.src.ViewModel
{
    public class PageTalla : ViewModelServiceBase<Talla>
    {
<<<<<<< HEAD
        public override ServiceBase<Talla> ServicioBase => new TallaService();

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
=======
        public override ServiceBase<Talla> ServicioBase => TallaService.Instance;
>>>>>>> 29/05/2025

        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }
}
