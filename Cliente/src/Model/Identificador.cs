using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Cliente.src.Attributes;
using Cliente.src.Extencions;
using Shared.Interfaces.Model.Obj;
using Shared.Interfaces.ModelsBase;
using Utilidades.Controls;

namespace Cliente.src.Model
{
    public class Identificador : ModelBase<IIdentificador>, IIdentificador
    {
        private string _name = string.Empty;
        private string _nickName = string.Empty;
        private string _descripcion = string.Empty;
        private string _value = string.Empty;
        private Atributo? _valorSeleccionado;
        private IEnumerable<Atributo> _valores = new ObservableCollection<Atributo>();

        [Solicitar("Nombre", ItemType = typeof(TextBox), InputBoxConvert = InputBoxType.Name)]
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
        [Solicitar("Descripcion", ItemType = typeof(TextBox), InputBoxConvert = InputBoxType.Text)]
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

        public Atributo? ValorSeleccionado
        {
            get => _valorSeleccionado;
            set => SetProperty(ref _valorSeleccionado, value);
        }

        public IEnumerable<Atributo> Valores
        {
            get => _valores;
            set => _valores = value;
        }

        IEnumerable<IAtributo> IIdentificador.Valores
        {
            get => Valores;
            set => Valores = value.Cast<Atributo>();
        }
        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
        public override void Update(IModelObj identity)
        {
            base.Update(identity);

            if (identity is Identificador entity)
            {
                Name = entity.Name;
                NickName = entity.NickName;
                Descripcion = entity.Descripcion;
                Value = entity.Value;

                if(Valores is ObservableCollection<Atributo> valoresCollection)
                    valoresCollection.ReplaceWith(entity.Valores);
            }
        }


    }
}
