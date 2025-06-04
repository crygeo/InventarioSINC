using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Interfaces;
using Shared.Interfaces.ModelsBase;

namespace Cliente.src.Model
{
    public class Identificador : ModelBase<IIdentificador>, IIdentificador
    {
        private string _name = string.Empty;
        private string _nickName = string.Empty;
        private string _descripcion = string.Empty;
        private string _value = string.Empty;
        private IEnumerable<IAtributo> _atributos = new List<Atributo>();


        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public string NickName
        {
            get => _nickName;
            set => SetProperty(ref _nickName, value);
        }
        public string Descripcion
        {
            get => _descripcion;
            set => SetProperty(ref _descripcion, value);
        }
        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
        public IEnumerable<IAtributo> Atributos
        {
            get => _atributos;
            set => SetProperty(ref _atributos, value);
        }


        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
        public override void Update(IModelObj identity)
        {
            throw new NotImplementedException();
        }
    }
}
