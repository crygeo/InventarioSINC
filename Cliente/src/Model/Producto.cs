using Shared.Interfaces;
using Shared.Interfaces.ModelsBase;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Cliente.src.Attributes;
using Cliente.src.View.Items;
using Utilidades.Controls;

namespace Cliente.src.Model
{
    public class Producto : ModelBase<IProducto>, IProducto
    {
        private string _name = string.Empty;
        private string _nickName = string.Empty;
        private string _descripcion = string.Empty;
        private IEnumerable<AtributosEntity> _atributos = new ObservableCollection<AtributosEntity>();
        private IEnumerable<Variantes> _variantes = new ObservableCollection<Variantes>();

        [Solicitar("Nombre",  Requerido = true, MinLength = 2, ItemType = typeof(TextBox), InputBoxConvert = InputBoxType.Name)]
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
        [Solicitar("Descripcion", ItemType = typeof(TextBox), InputBoxConvert = InputBoxType.Name)]
        public string Descripcion
        {
            get => _descripcion;
            set => SetProperty(ref _descripcion, value);
        }


        IEnumerable<IValorAtributo> IProducto.Atributos
        {
            get => Atributos;
            set => Atributos = value.Cast<AtributosEntity>();
        }

        [Solicitar("Atributos", MinItem = 1, ItemType = typeof(AtributesAdd), InputBoxConvert = InputBoxType.None)]
        public IEnumerable<AtributosEntity> Atributos
        {
            get => _atributos;
            set => SetProperty(ref _atributos, value);
        }

        IEnumerable<IVariantes> IProducto.Variantes
        {
            get => Variantes;
            set => Variantes = value.Cast<Variantes>();
        }
        [Solicitar("Variantes", MinItem = 1, ItemType = typeof(VariantesAdd), InputBoxConvert = InputBoxType.None)]  
        public IEnumerable<Variantes> Variantes
        {
            get => _variantes;
            set => SetProperty(ref _variantes, value);
        }

        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
        public override void Update(IModelObj entity)
        {
            base.Update(entity);

            if (entity is not IProducto producto)
                throw new ArgumentException("El objeto proporcionado no es un Producto válido.", nameof(entity));

            Name = producto.Name;
            NickName = producto.NickName;
            Descripcion = producto.Descripcion;
            Atributos = producto.Atributos.Cast<AtributosEntity>();
            Variantes = producto.Variantes.Cast<Variantes>();
        }
    }
}
