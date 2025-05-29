using Cliente.src.Services;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Interfaces;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades.Interfaces;
using Utilidades.Mvvm;

namespace Cliente.src.Model
{
    public class Usuario : ModelBase<IUsuario>, IUsuario
    {
        private string _primerNombre = "";
        public string PrimerNombre
        {
            get => _primerNombre;
            set => SetProperty(ref _primerNombre, value);
        }

        private string _segundoNombre = "";
        public string SegundoNombre
        {
            get => _segundoNombre;
            set => SetProperty(ref _segundoNombre, value);
        }

        private string _primerApellido = "";
        public string PrimerApellido
        {
            get => _primerApellido;
            set => SetProperty(ref _primerApellido, value);
        }

        private string _segundoApellido = "";
        public string SegundoApellido
        {
            get => _segundoApellido;
            set => SetProperty(ref _segundoApellido, value);
        }

        private string _cedula = "";
        public string Cedula
        {
            get => _cedula;
            set => SetProperty(ref _cedula, value);
        }

        private string _celular = "";
        public string Celular
        {
            get => _celular;
            set => SetProperty(ref _celular, value);
        }

        private DateTime _fechaNacimiento;
        public DateTime FechaNacimiento
        {
            get => _fechaNacimiento;
            set => SetProperty(ref _fechaNacimiento, value);
        }

        private string _user = "";
        public string User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private string _password = "";
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private List<string> _roles = [];
        public List<string> Roles
        {
            get => _roles;
            set => SetProperty(ref _roles, value);
        }
        private string _id = string.Empty;
        public override string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private bool _deleteable = false;
        public bool Deleteable
        {
            get => _deleteable;
            set => SetProperty(ref _deleteable, value);
        }

        public string NombreCompleto { get => $"{PrimerNombre} {SegundoNombre} {PrimerApellido} {SegundoApellido}"; set { } }
        public string NombreAndApellido { get => $"{PrimerNombre} {PrimerApellido}"; set { } }


        public override void Update(IIdentifiable identity)
        {
            if (identity is not IUsuario user)
                throw new ArgumentException("El objeto no es un usuario");

            PrimerNombre = user.PrimerNombre;
            SegundoNombre = user.SegundoNombre;
            PrimerApellido = user.PrimerApellido;
            SegundoApellido = user.SegundoApellido;
            Cedula = user.Cedula;
            Celular = user.Celular;
            FechaNacimiento = user.FechaNacimiento;
            User = user.User;
            Roles = user.Roles;
        }

        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }
}
