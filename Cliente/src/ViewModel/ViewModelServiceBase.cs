using Cliente.src.Services;
using Cliente.src.View.Dialog;
using MaterialDesignThemes.Wpf;
using Shared.Interfaces;
using Shared.Interfaces.Client;
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
    public abstract class ViewModelServiceBase<TEntity> : ViewModelBase where TEntity : IIdentifiable, ISelectable, IUpdate
    {
        public abstract ServiceBase<TEntity> ServicioBase { get; }
        public abstract ObservableCollection<TEntity> Entitys { get; }

        private TEntity? _entitySelect;
        public TEntity? EntiteSelect
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
            CargarEntityCommand = new RelayCommand((a) => CargarEntitys());
            CrearEntityCommand = new RelayCommand((a) => CrearEnttiy());
            EditarEntityCommand = new RelayCommand((a) => EditarEntity());
            EliminarEntityCommand = new RelayCommand((a) => DeleteEntity());

            InitAsync();
        }

        public virtual async void InitAsync()
        {
            var result = await ServicioBase.InitAsync();
            ValidarRespuesta(result);
        }

        public virtual async void StopAsync() => await ServicioBase.StopASunc();


        public virtual async void CargarEntitys()
        {
            if (_isLoading)
                return; // Si ya estamos cargando, no permitimos otra carga

            try
            {
                _isLoading = true;  // Marcamos como cargando
                ProgressVisible = true;  // Activamos la visualización del progreso
                ProgressValue = 0;  // Inicializamos el progreso

                // Comenzamos la tarea para simular el progreso mientras cargamos los usuarios
                var progressTask = Task.Run(async () =>
                {
                    for (int i = 0; i <= 100; i++)
                    {
                        ProgressValue = i;  // Actualiza el progreso
                        await Task.Delay(_tiempoEsperaS * 10);  // Ajustamos el tiempo para que el progreso dure 3 segundos
                    }
                });

                // Aquí cargamos los usuarios de forma asíncrona
                var entitys = await ServicioBase.GetAllAsync();


                Entitys.Clear();
                foreach (var entity in entitys.Item1)
                {
                    Entitys.Add(entity);
                }

                // Esperamos a que la tarea de progreso termine
                Entitys.FirstOrDefault()?.Select();
                await progressTask;

            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine($"Error al cargar usuarios: {ex.Message}");
            }
            finally
            {
                // Desactivar la visualización del progreso y restablecer el estado
                ProgressVisible = false;
                _isLoading = false;  // Permitimos que el método se ejecute nuevamente
                                     // Selecciona el primer usuario si existe
            }
        }

        public async void ValidarRespuesta((bool, string) resp)
        {
            if (resp.Item1)
                return;
            else
            {
                var errorDialog = new MessageDialog
                {
                    Message = resp.Item2,
                };

                await Task.Delay(300);
                await DialogHost.Show(errorDialog, "MainView");
            }
        }

        public abstract void CrearEnttiy();
        public abstract void EditarEntity();
        public abstract void DeleteEntity();
    }
}
