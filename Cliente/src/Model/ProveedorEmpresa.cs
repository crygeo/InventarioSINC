﻿using Shared.Interfaces;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades.Interfaces;

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

        public string RUC { get => _ruc; set => SetProperty(ref _ruc, value); }
        public string Direccion { get => _direccion; set => SetProperty(ref _direccion, value); }
        
        public string RazonSocial
        {
            get => _razonSocial;
            set => SetProperty(ref _razonSocial, value);
        }
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
