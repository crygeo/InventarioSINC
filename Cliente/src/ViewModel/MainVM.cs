using InventarioSINCliente.src.Model;
using InventarioSINCliente.src.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Utilidades.Command;
using Utilidades.Mvvm;

namespace InventarioSINCliente.src.ViewModel
{
    public class MainVM : ViewModelBase
    {
        protected override ModelBase Model { get; set; } = new MainM();
        private MainM MainM => (MainM)Model;


        private ViewModelBase _pageSelectViewModel;
        public ViewModelBase PageSelectViewModel
        {
            get => _pageSelectViewModel;
            set => SetProperty(ref _pageSelectViewModel, value, nameof(PageSelectViewModel));
        }

        private ICommand _changePageCommand;
        public ICommand ChangePageCommand
        {
            get => _changePageCommand;
            set => SetProperty(ref _changePageCommand, value, nameof(ChangePageCommand));
        }

        public MainVM()
        {
            _pageSelectViewModel = new PageUsuarioVM();
            _changePageCommand = new ChangedPageC(page => PageSelectViewModel = page);
        }

        
    }
}
