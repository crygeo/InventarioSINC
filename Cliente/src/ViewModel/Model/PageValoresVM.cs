using Cliente.ViewModel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cliente.Obj.Model;
using Cliente.Services;
using Cliente.Services.Model;
using CommunityToolkit.Mvvm.Input;

namespace Cliente.src.ViewModel.Model
{
    public class PageValoresVM : ViewModelServiceBase<ElementoJerarquico>
    {
        public ObservableCollection<Identificador> ListIdentificadors => ServiceFactory.GetService<Identificador>().Collection;
        public Dictionary<string, List<ElementoJerarquico>> ValoresPorIdentificador => Entitys
                .Where(v => !string.IsNullOrEmpty(v.IdPerteneciente))
                .GroupBy(v => v.IdPerteneciente)
                .ToDictionary(g => g.Key!, g => g.ToList());

        public IAsyncRelayCommand<Identificador> AddValorCommand { get; set; }

        public PageValoresVM()
        {
            AddValorCommand = new AsyncRelayCommand<Identificador>(crearValor);
        }

        public async Task crearValor(Identificador? identificador)
        {

           await DialogService.MostrarFormularioDinamicoAsync(new ElementoJerarquico(), $"Agregar nuevo \"{identificador.Name}\"", new AsyncRelayCommand<ElementoJerarquico>(aceptarCrearValor), DialogService.DialogIdentifierMain, DialogService.DialogSub01 );
            return;
        }

        private async Task aceptarCrearValor(ElementoJerarquico? elemnt)
        {

        }

        protected override void UpdateChanged()
        {
            base.UpdateChanged();
            OnPropertyChanged(nameof(ListIdentificadors));
            
        }
    }
}
