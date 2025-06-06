using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Cliente.src.Attributes;
using Utilidades.Controls;
using Utilidades.Interfaces;
using Shared.Interfaces.Model.Obj;
using Shared.Interfaces.Model;

namespace Cliente.src.Model
{
    public class ProveedorEmpresa : ModelBase<IProveedorEmpresa>, IProveedorEmpresa
    {
        private string _ruc = string.Empty;
        private string _direccion = string.Empty;


        #region Empresa

        private string _razonSocial = string.Empty;
        private string _representanteLegal = string.Empty;


        #endregion
        [Solicitar("RUC", ItemType = typeof(TextBox), InputBoxConvert = InputBoxType.Ruc)]
        public string RUC { get => _ruc; set => SetProperty(ref _ruc, value); }
        [Solicitar("Direccion", ItemType = typeof(TextBox), InputBoxConvert = InputBoxType.Direccion)]
        public string Direccion { get => _direccion; set => SetProperty(ref _direccion, value); }
        
        [Solicitar("Razon Social", ItemType = typeof(TextBox), InputBoxConvert = InputBoxType.Text)]
        public string RazonSocial
        {
            get => _razonSocial;
            set => SetProperty(ref _razonSocial, value);
        }
        [Solicitar("Representante Legal", ItemType = typeof(TextBox), InputBoxConvert = InputBoxType.Text)]
        public string RepresentanteLegal
        {
            get => _representanteLegal;
            set => SetProperty(ref _representanteLegal, value);
        }

       
        public override void Update(IModelObj identity)
        {
            if(identity is not IProveedorEmpresa proveedor)
                throw new ArgumentException("El objeto no es un proveedor válido.");

            Id = proveedor.Id;
            RUC = proveedor.RUC;
            Direccion = proveedor.Direccion;
            RazonSocial = proveedor.RazonSocial;
            RepresentanteLegal = proveedor.RepresentanteLegal;

        }
        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }
}
