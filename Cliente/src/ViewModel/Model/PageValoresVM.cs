using Cliente.Attributes;
using Cliente.Obj.Model;
using Cliente.Services;
using Cliente.Services.Model;
using Cliente.ViewModel.Model;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Cliente.Obj;
using MaterialDesignThemes.Wpf;
using Utilidades.Mvvm;

namespace Cliente.src.ViewModel.Model
{
    public class PageValoresVM : ViewModelServiceBase<ElementoJerarquico>
    {
        private ItemNavigationM _navigationSelect;

        public List<ItemNavigationM> Navigation { get; set; }
        public ItemNavigationM NavigationSelect
        {
            get => _navigationSelect;
            set => SetProperty(ref _navigationSelect, value);
        }

        public ObservableCollection<Identificador> ListIdentificadors => ServiceFactory.GetService<Identificador>().Collection;

        public Dictionary<string, List<ElementoJerarquico>> ValoresPorIdentificador => Entitys
                .Where(v => !string.IsNullOrEmpty(v.IdPerteneciente))
                .GroupBy(v => v.IdPerteneciente)
                .ToDictionary(g => g.Key!, g => g.OrderByDescending(e => e.FechaCreacion).ToList());


        public SubViewModelBase IdentificadoresSubModelBase { get; }
        public SubViewModelBase AtributosSubModelBase { get; }
        public PageValoresVM()
        {
            CrearEntityCommand = new AsyncRelayCommand<Identificador>(CrearEntityAsync);
            Entitys.CollectionChanged += ChangedCollection;

            IdentificadoresSubModelBase = new SubViewModelBase(this, "Identificadores");
            AtributosSubModelBase = new SubViewModelBase(this, "Atributos");

            Navigation = new List<ItemNavigationM>
            {
                new ItemNavigationM
                {
                    Title = "Identificadores",
                    SelectedIcon = PackIconKind.AlphaIBox,
                    UnselectedIcon = PackIconKind.AlphaIBoxOutline,
                    Page = IdentificadoresSubModelBase
                },
                new ItemNavigationM
                {
                    Title = "Atributos",
                    SelectedIcon = PackIconKind.AlphaABox,
                    UnselectedIcon = PackIconKind.AlphaABoxOutline,
                    Page = AtributosSubModelBase
                },
            };

            _navigationSelect = Navigation[0];

        }

        private void ChangedCollection(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ValoresPorIdentificador));
        }


        private async Task CrearEntityAsync(Identificador? identificador)
        {
            if (identificador == null) return;

            var newValor = new ElementoJerarquico
            {
                IdPerteneciente = identificador?.Id ?? string.Empty,
                Nombre = string.Empty,
                Descripcion = string.Empty
            };

            var nombresCampos = new Dictionary<string, string>
            {
                { nameof(newValor.Valor), identificador.Descripcion }
            };

            await DialogService.MostrarFormularioDinamicoAsync(newValor,
                    $"Agregar nuevo \"{identificador.Name}\"",
                    new AsyncRelayCommand<ElementoJerarquico>(ConfirmarCrearEntityAsync),
                    DialogService.DialogIdentifierMain, DialogService.DialogSub01,
                    nombresCampos);
        }



    }
}
