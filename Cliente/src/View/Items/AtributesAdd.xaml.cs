using Cliente.src.Attributes;
using Cliente.src.Extencions;
using Cliente.src.Model;
using Cliente.src.Services;
using Cliente.src.View.Dialog;
using Cliente.src.ViewModel;
using CommunityToolkit.Mvvm.Input;
using Shared.Extensions;
using Shared.Interfaces.ModelsBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cliente.src.View.Items
{
    /// <summary>
    /// Lógica de interacción para AtributesAdd.xaml
    /// </summary>
    public partial class AtributesAdd : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable<IValorAtributo>), typeof(AtributesAdd));
        public static readonly DependencyProperty EditarAtributoCommandProperty = DependencyProperty.Register(nameof(EditarAtributoCommand), typeof(IAsyncRelayCommand), typeof(AtributesAdd));
        public static readonly DependencyProperty EliminarAtributoCommandProperty = DependencyProperty.Register(nameof(EliminarAtributoCommand), typeof(IAsyncRelayCommand), typeof(AtributesAdd));
        public static readonly DependencyProperty EditarValorCommandProperty = DependencyProperty.Register(nameof(EditarValorCommand), typeof(IAsyncRelayCommand), typeof(AtributesAdd));
        public static readonly DependencyProperty EliminarValorCommandProperty = DependencyProperty.Register(nameof(EliminarValorCommand), typeof(IAsyncRelayCommand), typeof(AtributesAdd));

        public IEnumerable<IValorAtributo> ItemsSource
        {
            get => (IEnumerable<IValorAtributo>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        public IAsyncRelayCommand EditarAtributoCommand
        {
            get => (IAsyncRelayCommand)GetValue(EditarAtributoCommandProperty);
            set => SetValue(EditarAtributoCommandProperty, value);
        }
        public IAsyncRelayCommand EliminarAtributoCommand
        {
            get => (IAsyncRelayCommand)GetValue(EliminarAtributoCommandProperty);
            set => SetValue(EliminarAtributoCommandProperty, value);
        }
        public IAsyncRelayCommand EditarValorCommand
        {
            get => (IAsyncRelayCommand)GetValue(EditarValorCommandProperty);
            set => SetValue(EditarValorCommandProperty, value);
        }
        public IAsyncRelayCommand EliminarValorCommand
        {
            get => (IAsyncRelayCommand)GetValue(EliminarValorCommandProperty);
            set => SetValue(EliminarValorCommandProperty, value);
        }

        public AtributesAdd()
        {
            InitializeComponent();
            RegistrarComandos();
        }

        private async void AddAtributoEntity(object sender, RoutedEventArgs e)
        {

            var Dialog = new FormularioDinamico(new AtributosEntity())
            {
                AceptarCommand = new AsyncRelayCommand<AtributosEntity>(AgregarAtributo),
                TextHeader = $"Crear {ComponetesHelp.GetNombreEntidad<AtributosEntity>(Pluralidad.Singular)}",
                DialogOpenIdentifier = DialogService.DialogSub01,
                DialogNameIdentifier = DialogService.DialogSub02
            };

            await DialogService.Instance.MostrarDialogo(Dialog);
        }
        public Task AgregarAtributo(AtributosEntity? element)
        {
            if (element != null && ItemsSource is ObservableCollection<AtributosEntity> collection)
                collection.Add(element);
            return Task.CompletedTask;
        }


        private async void AddValueAtributo(object sender, RoutedEventArgs e)
        {
            if (sender is not Button bt)
                return;

            if (bt.DataContext is not AtributosEntity atributosEntity)
                return;

            var Dialog = new FormularioDinamico(new Atributo())
            {
                AceptarCommand = new AsyncRelayCommand<Atributo>((value) => AgregarValue(atributosEntity, value)),
                TextHeader = $"Crear {ComponetesHelp.GetNombreEntidad<Atributo>(Pluralidad.Singular)}",
                DialogOpenIdentifier = DialogService.DialogSub01,
                DialogNameIdentifier = DialogService.DialogSub02
            };

            await DialogService.Instance.MostrarDialogo(Dialog);
        }
        public Task AgregarValue(AtributosEntity atributo, Atributo? value)
        {
            if (value != null && atributo.Atributos is ObservableCollection<Atributo> collection)
                collection.Add(value);

            return Task.CompletedTask;
        }
        public void RegistrarComandos()
        {
            EditarAtributoCommand = new AsyncRelayCommand<AtributosEntity>(EditarAtributoAsync);

            EliminarAtributoCommand = new AsyncRelayCommand<AtributosEntity>(EliminarAtributoAsync);

            EditarValorCommand = new AsyncRelayCommand<Atributo>(EditarValorAsync);

            EliminarValorCommand = new AsyncRelayCommand<Atributo>(EliminarValorAsync);
        }

        private async Task EditarAtributoAsync(AtributosEntity? atributo)
        {
            if (atributo == null) return;

            await DialogService.MostrarFormularioDinamicoAsyncSubDialog01(atributo.Clone(), "Editar atributo", (a) =>
            {
                atributo.Name = a.Name;
                return Task.CompletedTask;
            });

        }

        private Task EliminarAtributoAsync(AtributosEntity? atributo)
        {
            if (atributo == null) return Task.CompletedTask;

            if (ItemsSource is ObservableCollection<AtributosEntity> collection)
                collection.Remove(atributo);

            return Task.CompletedTask;
        }

        private async Task EditarValorAsync(Atributo? valor)
        {

            if (valor == null) return;

            await DialogService.MostrarFormularioDinamicoAsyncSubDialog01(valor.Clone(), "Editar valor", (a) =>
            {
                valor.Value = a.Value;
                return Task.CompletedTask;
            });

        }

        private Task EliminarValorAsync(Atributo? valor)
        {
            if (valor == null) return Task.CompletedTask;

            var atributosParent = ItemsSource.OfType<AtributosEntity>()
                .FirstOrDefault(a => a.Atributos.Contains(valor));

            if (atributosParent?.Atributos is ObservableCollection<Atributo> collection)
                collection.Remove(valor);

            return Task.CompletedTask;
        }



    }
}
