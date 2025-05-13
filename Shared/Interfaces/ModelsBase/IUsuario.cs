using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.ModelsBase
{
    public interface IUsuario : IPersona
    {
        string User { get; set; }
        string Password { get; set; }
        List<string> Roles { get; set; }
    }

}
