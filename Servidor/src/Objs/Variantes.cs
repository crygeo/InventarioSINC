using Shared.Interfaces.ModelsBase;
using System.Collections.Generic;
using System.Linq;

namespace Servidor.src.Objs
{
    public class Variantes : IVariantes
    {
        private string _value { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Descripcion { get; set; }

        public string Value
        {
            get => _value;
            set => _value = value;
        }
    }
}
