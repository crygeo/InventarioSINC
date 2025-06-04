using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.ModelsBase
{
    public interface IAtributosEntity : INameDescrition
    {
        IEnumerable<IAtributo> Atributos { get; set; }

    }
}
