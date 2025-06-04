using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.ModelsBase
{
    public interface IProveedor : IModelObj
    {
        string RUC { get; set; }
        string Direccion { get; set; }
    }
}
