using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Cliente.src.Attributes;
using Shared.Interfaces.ModelsBase;

namespace Cliente.src.Model
{
    [NombreEntidad("Atributo")]
    public class AtributosEntity : ModelBase, IAtributosEntity
    {
        private string _name = string.Empty;
        private string _nickName = string.Empty;
        private string _descripcion = string.Empty;
        private IEnumerable<Atributo> _atributos = new ObservableCollection<Atributo>();


        
        [Solicitar("Nombre", Requerido = true, ItemType = typeof(TextBox))]
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

        IEnumerable<IAtributo> IAtributosEntity.Atributos
        {
            get => Atributos;
            set => Atributos = value.Cast<Atributo>();
        }
        public IEnumerable<Atributo> Atributos
        {
            get => _atributos;
            set => SetProperty(ref _atributos, value);
        }
        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }

    }
}
