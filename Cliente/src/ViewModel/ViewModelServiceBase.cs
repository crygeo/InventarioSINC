using Cliente.src.Extencions;
using Cliente.src.Model;
using Cliente.src.Services;
using Cliente.src.View.Dialog;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Utilidades.Interfaces;
using Utilidades.Mvvm;

namespace Cliente.src.ViewModel
{
    public abstract class ViewModelServiceBase<TEntity> : ViewModelBase where TEntity : IIdentifiable, IUpdate, ISelectable
    {
        protected DialogService DialogService { get; set; } = DialogService.Instance;

        public abstract ServiceBase<TEntity> ServicioBase { get; }
        public ObservableCollection<TEntity> Entitys { get => ServicioBase.Collection; }

        private TEntity? _entitySelect;
        public TEntity? EntitySelect
        {
            get { return _entitySelect; }
            set { SetProperty(ref _entitySelect, value); }
        }

        private bool _progressVisible;
        public bool ProgressVisible
        {
            get => _progressVisible;
            set => SetProperty(ref _progressVisible, value);
        }
        private int _progressValue;
        public int ProgressValue
        {
            get => _progressValue;
            set => SetProperty(ref _progressValue, value);
        }

        private bool _autoCarga = true;
        public bool AutoCarga
        {
            get => _autoCarga;
            set => SetProperty(ref _autoCarga, value);
        }

        private bool _isLoading = false; // Variable para controlar si ya estamos en el proceso de carga
        private int _tiempoEsperaS = 5; // Variable para controlar si ya estamos en el proceso de carga


        public ICommand CargarEntityCommand { get; }
        public ICommand CrearEntityCommand { get; }
        public ICommand EditarEntityCommand { get; }
        public ICommand EliminarEntityCommand { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeService">Tipo del servicio para crear la instacia del servicio, debe ser de tipo <see cref="ServiceBase"/> para que funcione</param>
        /// <exception cref="Exception">Si el tipo no es correcto lanzara un error</exception>
        protected ViewModelServiceBase()
        {
            CargarEntityCommand = new AsyncRelayCommand((a) => CargarEntitysAsync());
            CrearEntityCommand = new AsyncRelayCommand((a) => CrearEntityAsync());
            EditarEntityCommand = new AsyncRelayCommand((a) => EditarEntityAsync());
            EliminarEntityCommand = new AsyncRelayCommand((a) => DeleteEntityAsync());

            InitAsync();
        }

        public virtual async void InitAsync()
        {
            var result = await ServicioBase.InitAsync();
            await DialogService.ValidarRespuesta(result);
        }


        public virtual async void StopAsync() => await ServicioBase.StopAsync();


        public virtual async Task CargarEntitysAsync()
        {
            if (_isLoading)
                return;

            _isLoading = true;
            ProgressVisible = true;
            ProgressValue = 0;

            var progressTask = Task.Run(async () =>
            {
                for (int i = 0; i <= 100; i++)
                {
                    ProgressValue = i;
                    await Task.Delay(_tiempoEsperaS * 10);
                }

                ProgressVisible = false;
                _isLoading = false;
            });

            var result = await ServicioBase.UpdateCollection();
            await DialogService.ValidarRespuesta(result);
            if (result.Success)
                Entitys.FirstOrDefault()?.Select();

            await progressTask;

        }

        public abstract Task CrearEntityAsync();
        public abstract Task EditarEntityAsync();
        public abstract Task DeleteEntityAsync();

    }
}
