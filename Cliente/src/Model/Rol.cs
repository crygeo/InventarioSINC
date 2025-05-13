using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Interfaces.ModelsBase;
using Shared.Interfaces.Client;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Utilidades.Interfaces;
using System.Security.Principal;
namespace Cliente.src.Model
{
    public class Rol : NotifyPropertyChanged, IRol, ISelectable, IClassified, IUpdate
    {
        private string _id = string.Empty;
        public string Id { get => _id; set => SetProperty(ref _id, value); }

        private string _nombre = "";
        public string Nombre { get => _nombre; set => SetProperty(ref _nombre, value); }


        private List<string> _permisos = [];
        public List<string> Permisos { get => _permisos; set => SetProperty(ref _permisos, value); }


        public bool _isAdmin = false;
        public bool IsAdmin { get => _isAdmin; set => SetProperty(ref _isAdmin, value); }


        private bool _isSlectable = true;
        public bool IsSelectable { get => _isSlectable; set => SetProperty(ref _isSlectable, value); }


        private bool _isSelect = false;
        public bool IsSelect { get => _isSelect; set => SetProperty(ref _isSelect, value); }

        private bool _isClassified = false;
        public bool IsClassified { get => _isClassified; set => SetProperty(ref _isClassified, value); }

        private string _message = string.Empty;
        public string Message { get => _message; set => SetProperty(ref _message, value); }

        public void Update(IIdentifiable identity)
        {
            if(identity is IRol rol)
            {
                Nombre = rol.Nombre;
                Permisos = rol.Permisos;
                IsAdmin = rol.IsAdmin;
            }
        }
        
    }
}
