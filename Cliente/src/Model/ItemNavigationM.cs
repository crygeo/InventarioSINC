using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades.Mvvm;

namespace Cliente.src.Model
{
    public class ItemNavigationM : ViewModelBase
    {
        public required string Title { get; set; }
        public PackIconKind SelectedIcon { get; set; }
        public PackIconKind UnselectedIcon { get; set; }

        public required ViewModelBase Page { get; set; }

        private object? _notification = null;


        public object? Notification
        {
            get { return _notification; }
            set { SetProperty(ref _notification, value); }
        }
    }
}
