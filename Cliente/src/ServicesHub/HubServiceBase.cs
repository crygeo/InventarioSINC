using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Cliente.src.Model;
using System.Windows;
using System.Collections.ObjectModel;
using Cliente.src.Converter;
using Cliente.src.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Shared.Interfaces.ModelsBase;
using Utilidades.Interfaces;

namespace Cliente.src.ServicesHub
{

    public class HubServiceBase<TEntity> : Utilidades.Interfaces.IHubService<TEntity> where TEntity : IModelObj
    {
        protected readonly HubConnection _hubConnection;
        private readonly string _hubUrl;
        public virtual ObservableCollection<TEntity> Collection { get; } = []; 

        public HubServiceBase()
        {
            _hubUrl = $"{Config.Url}/hub{typeof(TEntity).Name}";

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_hubUrl) // URL del Hub en el servidor
                .WithAutomaticReconnect() // Permite la reconexión automática
                .Build();

            SubscribeToEvent<TEntity>($"New{typeof(TEntity).Name}", (entity) =>
            {
                Application.Current.Dispatcher.Invoke(() => Collection.Add(entity));
            });

            SubscribeToEvent<TEntity>($"Update{typeof(TEntity).Name}", (entity) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var existingUser = Collection.FirstOrDefault(u => u.Id == entity.Id);
                    if (existingUser != null)
                    {
                        existingUser.Update(entity);
                    }
                });
            });

            SubscribeToEvent<TEntity>($"Delete{typeof(TEntity).Name}", (entity) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var usuario = Collection.FirstOrDefault(u => u.Id == entity.Id);
                    if (usuario != null)
                    {
                        Collection.Remove(usuario); // Eliminar usuario de la lista
                    }
                });
            });
        }

        /// <summary>
        /// Método para suscribirse a eventos de SignalR de forma flexible
        /// </summary>
        protected void SubscribeToEvent<U>(string methodName, Action<U> action)
        {
            _hubConnection.On(methodName, action);
        }

        /// <summary>
        /// Inicia la conexión con el servidor SignalR.
        /// </summary>
        public async Task StartConnectionAsync()
        {
            try
            {
                await _hubConnection.StartAsync();
                Console.WriteLine($"Conectado a {_hubUrl}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar con {_hubUrl}: {ex.Message}");
            }
        }

        /// <summary>
        /// Detiene la conexión con el servidor SignalR.
        /// </summary>
        public async Task StopConnectionAsync()
        {
            await _hubConnection.StopAsync();
        }
    }
}

