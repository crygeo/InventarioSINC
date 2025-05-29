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
<<<<<<< HEAD
    public interface IUsuario : IPersona, IIdentifiable
=======
    public interface IUsuario : IPersona, IIdentifiable, IDeleteable
>>>>>>> 29/05/2025
    {
        string User { get; set; }
        string Password { get; set; }
        List<string> Roles { get; set; }
    }

}
