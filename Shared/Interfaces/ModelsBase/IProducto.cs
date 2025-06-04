using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.ModelsBase
{
    public interface IProducto : IModelObj, INameDescrition
    {
        IEnumerable<IAtributosEntity> Atributos { get; set; }
        IEnumerable<IVariantes> Variantes { get; set; }
    }
}
