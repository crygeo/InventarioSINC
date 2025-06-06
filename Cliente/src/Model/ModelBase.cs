using Cliente.src.Extencions;
using Shared.Interfaces;
using Shared.Interfaces.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Utilidades.Interfaces;

namespace Cliente.src.Model
{
    public abstract class ModelBase<T> : ModelBase, ISelectable, IModelObj where T : IModelObj
    {

        private bool _isSelectable = true;

        public bool IsSelectable
        {
            get => _isSelectable;
            set => SetProperty(ref _isSelectable, value);
        }

        private string _id = "";
        private bool _isSelect = false;
        private bool _deleteable = true;
        private bool _updatable;

        public bool IsSelect
        {
            get => _isSelect;
            set => SetProperty(ref _isSelect, value);
        }

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public bool Deleteable
        {
            get => _deleteable;
            set => SetProperty(ref _deleteable, value);
        }

        public bool Updatable
        {
            get => _updatable;
            set => SetProperty(ref _updatable, value);
        }

        public virtual void Update(IModelObj entity)
        {
            Id = entity.Id;
            Deleteable = entity.Deleteable;
            Updatable = entity.Updatable;
        }

        
    }



    public abstract class ModelBase : NotifyProperty, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errores = new();

        public bool HasErrors => _errores.Any();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) return _errores.SelectMany(e => e.Value);
            return _errores.TryGetValue(propertyName, out var errores) ? errores : Enumerable.Empty<string>();
        }

        protected override bool SetProperty<T1>(ref T1 field, T1 value, [CallerMemberName] string? propertyName = null)
        {
            var cambiado = base.SetProperty(ref field, value, propertyName);
            if (cambiado)
                Validar(propertyName!);
            return cambiado;
        }

        private void Validar(string propertyName)
        {
            _errores.Remove(propertyName);

            var nuevosErrores = this.ValidarPropiedadAtributo(propertyName);

            if (nuevosErrores.Any())
                _errores[propertyName] = nuevosErrores;

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}