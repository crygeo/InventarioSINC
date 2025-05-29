<<<<<<< HEAD
﻿using Cliente.src.Extencions;
using Cliente.src.Model;
using Cliente.src.Services;
using Cliente.src.View.Dialog;
=======
﻿using Cliente.src.Attributes;
using Cliente.src.Extencions;
using Cliente.src.Model;
using Cliente.src.Services;
using Cliente.src.View.Dialog;
using Cliente.src.View.Items;
>>>>>>> 29/05/2025
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Shared.Extensions;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Utilidades.Interfaces;
using Utilidades.Mvvm;

namespace Cliente.src.ViewModel
{
<<<<<<< HEAD
    public abstract class ViewModelServiceBase<TEntity> : ViewModelBase where TEntity : IIdentifiable, IUpdate, ISelectable
=======
    public abstract class ViewModelServiceBase<TEntity> : ViewModelBase where TEntity : IIdentifiable, IUpdate, ISelectable, new()
>>>>>>> 29/05/2025
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

        public Type TypeEntity { get => typeof(TEntity); }

        private bool _isLoading = false; // Variable para controlar si ya estamos en el proceso de carga
        private int _tiempoEsperaS = 5; // Variable para controlar si ya estamos en el proceso de carga


        public IAsyncRelayCommand CargarEntityCommand { get; }
        public IAsyncRelayCommand CrearEntityCommand { get; }
        public IAsyncRelayCommand EditarEntityCommand { get; }
        public IAsyncRelayCommand EliminarEntityCommand { get; }

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

<<<<<<< HEAD

        public virtual async void StopAsync() => await ServicioBase.StopAsync();
=======
>>>>>>> 29/05/2025

        public virtual async void StopAsync() => await ServicioBase.StopAsync();

<<<<<<< HEAD
=======

>>>>>>> 29/05/2025
        public virtual async Task CargarEntitysAsync()
        {
            if (_isLoading)
                return;
<<<<<<< HEAD

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

=======

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


        public async virtual Task CrearEntityAsync()
        {
            var Dialog = new FormularioDinamico(new TEntity())
            {
                AceptarCommand = new AsyncRelayCommand<TEntity>(AgregarUsuario),
                TextHeader = $"Crear {GetNombreEntidad(Pluralidad.Singular)}"
            };

            await DialogService.MostrarDialogo(Dialog);
        }
        public async virtual Task EditarEntityAsync()
        {
            if (EntitySelect == null)
                return;

            var Dialog = new FormularioDinamico(EntitySelect.Clone())
            {
                AceptarCommand = new AsyncRelayCommand<TEntity?>(ConfirmarEditarEntityAsync),
                TextHeader = $"Editar {GetNombreEntidad(Pluralidad.Singular)}"
            };

            await DialogService.MostrarDialogo(Dialog);
        }
        public async virtual Task DeleteEntityAsync()
        {
            if (EntitySelect == null)
                return;

            var confirmDialog = new ConfirmDialog
            {
                TextHeader = $"Eliminar {GetNombreEntidad(Pluralidad.Singular)}",
                Message = $"¿Estás seguro de que quieres eliminar el {GetNombreEntidad(Pluralidad.Singular)} seleccionado?",
                AceptedCommand = new AsyncRelayCommand(ConfirmarEliminarEntityAsync)
            };

            await DialogService.MostrarDialogo(confirmDialog);
        }


        private async Task AgregarUsuario(TEntity? entity)
        {
            if (entity == null)
                return;

            await DialogService.MostrarDialogoProgreso(async () =>
            {
                var result = await ServicioBase.CreateAsync(entity);
                await DialogService.ValidarRespuesta(result);
                return result;
            });
        }
        private async Task ConfirmarEditarEntityAsync(TEntity? entity)
        {
            if (entity == null) return;

            await DialogService.MostrarDialogoProgreso(async () =>
            {
                var result = await ServicioBase.UpdateAsync(entity.Id, entity);
                await DialogService.ValidarRespuesta(result);
                return result;
            });
        }
        private async Task ConfirmarEliminarEntityAsync()
        {

            if (EntitySelect == null) return;

            await DialogService.MostrarDialogoProgreso(async () =>
            {
                var result = await ServicioBase.DeleteAsync(EntitySelect.Id);
                await DialogService.ValidarRespuesta(result);
                return result;
            });
        }

        private string GetNombreEntidad(Pluralidad pluralidad)
        {
            string nombreEntidad = "";

            var atributo = typeof(TEntity).GetCustomAttribute<NombreEntidadAttribute>();

            switch (pluralidad)
            {
                case Pluralidad.Plural:
                    nombreEntidad = atributo?.Plural ?? $"{typeof(TEntity).Name.ToLower()}s";
                    break;
                case Pluralidad.Singular:
                    nombreEntidad = atributo?.Singular ?? typeof(TEntity).Name.ToLower();
                    break;

            }

            return nombreEntidad;
        }

        private enum Pluralidad
        {
            Singular,
            Plural
        }
>>>>>>> 29/05/2025
    }
}
