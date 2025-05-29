using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IDeleteable
    {
        bool Deleteable { get; set; } // Indica si el objeto es eliminable o no
    }
}
