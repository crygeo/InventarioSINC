using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Interfaces.ModelsBase;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Utilidades.Interfaces;
using System.Security.Principal;
using Shared.Interfaces.Model.Obj;
namespace Cliente.src.Model
{
    public class Rol : ModelBase<IRol>, IRol, IClassified
    {
        private string _nombre = "";
        public string Nombre { get => _nombre; set => SetProperty(ref _nombre, value); }


        private List<string> _permisos = [];
        public List<string> Permisos { get => _permisos; set => SetProperty(ref _permisos, value); }


        public bool _isAdmin = false;
        public bool IsAdmin { get => _isAdmin; set => SetProperty(ref _isAdmin, value); }

        private bool _isClassified = false;
        public bool IsClassified { get => _isClassified; set => SetProperty(ref _isClassified, value); }

        private string _message = string.Empty;
        public string Message { get => _message; set => SetProperty(ref _message, value); }


        public override void Update(IModelObj identity)
        {
            if(identity is not IRol rol)
                throw new ArgumentException("El objeto no es un rol válido.");

            Id = rol.Id;
            Nombre = rol.Nombre;
            Permisos = rol.Permisos;
            IsAdmin = rol.IsAdmin;

        }

        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }
}
