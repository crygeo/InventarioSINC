using Cliente.src.Attributes;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Utilidades.Controls;

namespace Cliente.src.Model
{

    [NombreEntidad("Entidad")]
    public class Atributo : ModelBase, IAtributo
    {
        private string _name = string.Empty;
        private string _nickName = string.Empty;
        private string _descripcion = string.Empty;
        private string _value = string.Empty;


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

        [Solicitar("Valor", Requerido = true, ItemType = typeof(TextBox), InputBoxConvert = InputBoxType.Text, MaxLength = 20)]
        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }

}
