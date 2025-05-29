using Cliente.src.Model;
using Cliente.src.Services;
<<<<<<< HEAD
=======
using Cliente.src.View.Dialog;
>>>>>>> 29/05/2025
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.src.ViewModel
{
    public class PageCalidad : ViewModelServiceBase<Calidad>
    {
<<<<<<< HEAD
        public override ServiceBase<Calidad> ServicioBase => new CalidadService();

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
        public override ServiceBase<Calidad> ServicioBase => CalidadService.Instance;
>>>>>>> 29/05/2025
        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }
}
