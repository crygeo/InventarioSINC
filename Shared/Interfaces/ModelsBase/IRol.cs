using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using Shared.Interfaces;

namespace Shared.Interfaces.ModelsBase
{
    public interface IRol : IIdentifiable, IDeleteable
    {
        string Nombre { get; set; } // Ejemplo: "Administrador", "Editor", "Usuario"
        List<string> Permisos { get; set; } // Ejemplo: ["Usuarios.Crear", "Usuarios.Eliminar"]
        bool IsAdmin { get; set; }
    }
}
