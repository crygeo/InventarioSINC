using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.Model
{
    public interface IProveedor
    {
        string RUC { get; set; }
        string Direccion { get; set; }
    }
}
