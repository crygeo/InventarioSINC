using Shared.Interfaces.ModelsBase;
using System;

namespace Servidor.src.Objs
{
    public class ProveedorEmpresa : IProveedor, IEmpresa
    {
        public string RUC { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string Direccion { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string Id { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string RazonSocial { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string RepresentanteLegal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

    public class ProveedorPersona : IProveedor, IPersona
    {
        public string RUC { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string Direccion { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string Id { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public string PrimerNombre => throw new NotImplementedException();

        public string SegundoNombre => throw new NotImplementedException();

        public string PrimerApellido => throw new NotImplementedException();

        public string SegundoApellido => throw new NotImplementedException();

        public string Cedula => throw new NotImplementedException();

        public string Celular => throw new NotImplementedException();

        public DateTime FechaNacimiento => throw new NotImplementedException();
    }
}
