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
    public class PageClase : ViewModelServiceBase<Clase>
    {
<<<<<<< HEAD
        public override ServiceBase<Clase> ServicioBase => new ClaseService();

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
        public override ServiceBase<Clase> ServicioBase => ClaseService.Instance;
>>>>>>> 29/05/2025

        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }
}
