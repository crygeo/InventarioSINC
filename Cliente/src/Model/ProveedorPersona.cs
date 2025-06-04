using Shared.Interfaces;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades.Interfaces;

namespace Cliente.src.Model
{
    public class ProveedorPersona : ModelBase<IProveedorPersona>, IProveedorPersona
    {
        private string _id = string.Empty;
        private string _ruc = string.Empty;
        private string _direccion = string.Empty;


        #region Persona

        private string _primerNombre = string.Empty;
        private string _segundoNombre = string.Empty;
        private string _primerApellido = string.Empty;
        private string _segundoApellido = string.Empty;
        private string _cedula = string.Empty;
        private string _celular = string.Empty;
        private DateTime _fechaNacimiento = DateTime.Now;

        #endregion

        public string RUC { get => _ruc; set => SetProperty(ref _ruc, value); }
        public string Direccion { get => _direccion; set => SetProperty(ref _direccion, value); }


        public string PrimerNombre
        {
            get => _primerNombre;
            set => SetProperty(ref _primerNombre, value);
        }

        public string SegundoNombre
        {
            get => _segundoNombre;
            set => SetProperty(ref _segundoNombre, value);
        }

        public string PrimerApellido
        {
            get => _primerApellido;
            set => SetProperty(ref _primerApellido, value);
        }
        public string SegundoApellido
        {
            get => _segundoApellido;
            set => SetProperty(ref _segundoApellido, value);
        }

        public string Cedula
        {
            get => _cedula;
            set => SetProperty(ref _cedula, value);
        }

        public string Celular
        {
            get => _celular;
            set => SetProperty(ref _cedula, value);
        }

        public DateTime FechaNacimiento
        {
            get => _fechaNacimiento;
            set => SetProperty(ref _fechaNacimiento, value);
        }
        
        public override void Update(IModelObj identity)
        {
            if(identity is not IProveedor proveedor)
                throw new ArgumentException("El objeto no es un proveedor válido.");
            Id = proveedor.Id;
            RUC = proveedor.RUC;
            Direccion = proveedor.Direccion;
        }
        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }
}
