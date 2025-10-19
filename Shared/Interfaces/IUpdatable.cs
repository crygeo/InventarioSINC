using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Shared.Interfaces.Model;

namespace Shared.Interfaces
{
    public interface IUpdatable
    {
        bool Updatable { get; set; }
        void Update(IModelObj entity);
    }
}
