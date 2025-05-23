using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades.Interfaces;

namespace Cliente.src.Model
{
    public abstract class ModelBase<T> : NotifyProperty, ISelectable, IUpdate, IIdentifiable
    where T : IIdentifiable
    {
        private bool _isSelectable = true;
        public bool IsSelectable { get => _isSelectable; set => SetProperty(ref _isSelectable, value); }

        private bool _isSelect = false;
        public bool IsSelect { get => _isSelect; set => SetProperty(ref _isSelect, value); }
        public abstract string Id { get; set; }

        public abstract void Update(IIdentifiable identity);
       
    }



}
