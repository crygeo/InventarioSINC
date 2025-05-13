using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IUpdate
    {
        void Update(IIdentifiable identity);
    }
}
