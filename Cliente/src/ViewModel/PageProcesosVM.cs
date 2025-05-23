using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Cliente.src.Model;
using Cliente.src.Services;
using Cliente.src.View.Dialog;
using Cliente.src.View.Items;
using MaterialDesignThemes.Wpf;
using Shared.Extensions;
using Utilidades.Mvvm;

namespace Cliente.src.ViewModel
{
    public class PageProcesosVM : ViewModelBase
    {

        private ViewModelBase _pageSelectViewModel = null!;
        public ViewModelBase PageSelectViewModel
        {
            get => _pageSelectViewModel;
            set => SetProperty(ref _pageSelectViewModel, value);
        }

        private ItemNavigationM _selectedItemNav = null!;
        public ItemNavigationM SelectedItemNav
        {
            get => _selectedItemNav;
            set
            {
                if (SetProperty(ref _selectedItemNav, value))
                {
                    PageSelectViewModel = value.Page;
                }
            }
        }

        public List<ItemNavigationM> ListItemsNav { get; } = [
           new() {
                Title = "Color",
                SelectedIcon = PackIconKind.AlphaABox,
                UnselectedIcon = PackIconKind.AlphaABoxOutline,
                Notification = 1,
                Page = new PageCalidad()
            },
            new() {
                Title = "Clase",
                SelectedIcon = PackIconKind.AlphaBBox,
                UnselectedIcon = PackIconKind.AlphaBBoxOutline,
                Page = new PageClase()
            },
            new() {
                Title = "Liquidacion",
                SelectedIcon = PackIconKind.AlphaCBox,
                UnselectedIcon = PackIconKind.AlphaCBoxOutline,
                Page = new PageClasificacion()
            },
            new() {
                Title = "Proveedor",
                SelectedIcon = PackIconKind.AlphaDBox,
                UnselectedIcon = PackIconKind.AlphaDBoxOutline,
                Page = new PageProveedor()
            },
            new() {
                Title = "Recepcion",
                SelectedIcon = PackIconKind.AlphaEBox,
                UnselectedIcon = PackIconKind.AlphaEBoxOutline,
                Page = new PageRecepcionCarga()
            },
            new() {
                Title = "Talla",
                SelectedIcon = PackIconKind.AlphaFBox,
                UnselectedIcon = PackIconKind.AlphaFBoxOutline,
                Page = new PageTalla()
            },
           ];

        // Constructor, inicializa el servicio y carga los usuarios
        public PageProcesosVM()
        {
        }

        protected override void UpdateChanged()
        {
            throw new NotImplementedException();
        }
    }
}
