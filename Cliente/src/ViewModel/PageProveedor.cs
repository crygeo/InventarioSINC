using Cliente.src.Model;
using Cliente.src.Services.Model;
using Shared.Interfaces;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cliente.src.Services;

namespace Cliente.src.ViewModel
{
    public class PageProveedorEmpresa : ViewModelServiceBase<ProveedorEmpresa>
    {
        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }

    public class PageProveedorPersona : ViewModelServiceBase<ProveedorPersona>
    {
        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }
}
