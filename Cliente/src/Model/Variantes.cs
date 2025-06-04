using Shared.Interfaces.ModelsBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Cliente.src.Attributes;
using MaterialDesignThemes.Wpf;

namespace Cliente.src.Model
{
    public class Variantes : ModelBase, IVariantes
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

        [Solicitar("Variante", Requerido = true, ItemType = typeof(TextBox))]
        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        [Solicitar("Descripcion", Requerido = true, ItemType = typeof(TextBox))]
        public string Descripcion
        {
            get => _descripcion;
            set => SetProperty(ref _descripcion, value);
        }

        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }
}
