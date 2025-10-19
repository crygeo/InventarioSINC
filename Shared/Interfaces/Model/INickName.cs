using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.ModelsBase
{
    public interface INickName
    {
        string Name { get; set; }
        string NickName { get; set; }
        string DisplayName
        {
            get => string.IsNullOrEmpty(NickName) ? Name : NickName;
        }
    }
}
