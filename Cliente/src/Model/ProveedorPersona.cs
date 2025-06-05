using Cliente.src.Attributes;
using Shared.Interfaces;
using Shared.Interfaces.Model;
using Shared.Interfaces.Model.Obj;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Utilidades.Controls;
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
        [Solicitar("Primer Nombre", ItemType = typeof(TextBox), InputBoxConvert = InputBoxType.Name)]
        public string PrimerNombre
        {
            get => _primerNombre;
            set => SetProperty(ref _primerNombre, value);
        }

        [Solicitar("Segundo Nombre", ItemType = typeof(TextBox), InputBoxConvert = InputBoxType.Name)]
        public string SegundoNombre
        {
            get => _segundoNombre;
            set => SetProperty(ref _segundoNombre, value);
        }

        [Solicitar("Primer Apellido", ItemType = typeof(TextBox), InputBoxConvert = InputBoxType.Name)]
        public string PrimerApellido
        {
            get => _primerApellido;
            set => SetProperty(ref _primerApellido, value);
        }

        [Solicitar("Segundo Apellido", ItemType = typeof(TextBox), InputBoxConvert = InputBoxType.Name)]
        public string SegundoApellido
        {
            get => _segundoApellido;
            set => SetProperty(ref _segundoApellido, value);
        }

        [Solicitar("RUC", ItemType = typeof(TextBox), InputBoxConvert = InputBoxType.Ruc)]
        public string RUC
        {
            get => _ruc; set => SetProperty(ref _ruc, value);
        }

        [Solicitar("Direccion", ItemType = typeof(TextBox), InputBoxConvert = InputBoxType.Text)]
        public string Direccion
        {
            get => _direccion; set => SetProperty(ref _direccion, value);
        }

        [Solicitar("Cedula", ItemType = typeof(TextBox), InputBoxConvert = InputBoxType.Dni)]
        public string Cedula
        {
            get => _cedula;
            set => SetProperty(ref _cedula, value);
        }

        [Solicitar("Telefono", ItemType = typeof(TextBox), InputBoxConvert = InputBoxType.Phone)]
        public string Celular
        {
            get => _celular;
            set => SetProperty(ref _celular, value);
        }

        [Solicitar("Fecha de Nacimiento", ItemType = typeof(DatePicker), InputBoxConvert = InputBoxType.None)]
        public DateTime FechaNacimiento
        {
            get => _fechaNacimiento;
            set => SetProperty(ref _fechaNacimiento, value);
        }





        public override void Update(IModelObj identity)
        {
            if (identity is not IProveedor proveedor)
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
