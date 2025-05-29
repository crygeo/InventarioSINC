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
    public class PageRecepcionCarga : ViewModelServiceBase<RecepcionCarga>
    {
<<<<<<< HEAD
        public override ServiceBase<RecepcionCarga> ServicioBase => new RecepcionCargaService();

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
        public override ServiceBase<RecepcionCarga> ServicioBase => RecepcionCargaService.Instance;
>>>>>>> 29/05/2025

        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }
}
