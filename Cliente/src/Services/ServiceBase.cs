using Cliente.src.Extencions;
using Cliente.src.Model;
using Cliente.src.ServicesHub;
using MongoDB.Bson;
using Newtonsoft.Json;
using Shared.Interfaces;
using Shared.ObjectsResponse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Utilidades.Interfaces;

namespace Cliente.src.Services
{
    public abstract class ServiceBase<TEntity> : HttpClientBase, IServiceClient<TEntity>
    where TEntity : IIdentifiable, IUpdate
    {
        public readonly Utilidades.Interfaces.IHubService<TEntity> HubService;

        public ObservableCollection<TEntity> Collection => HubService.Collection;

        public override string BaseUrl { get; } = $"{Config.FullUrl}/{typeof(TEntity).Name}";

        protected ServiceBase(Utilidades.Interfaces.IHubService<TEntity> hubService)
        {
            HubService = hubService;
        }

        public virtual async Task<IResultResponse<bool>> InitAsync()
        {
            await HubService.StartConnectionAsync();
            Collection.Clear();

            var result = await GetAllAsync();
            if (!result.Success || result.Entity == null)
                return ResultError<bool>(result.Message, result.Error);

            Collection.ReplaceWith(result.Entity);

            return ResultSuccess(true, "Colección inicializada");
        }

        public virtual async Task<IResultResponse<bool>> StopAsync()
        {
            Collection.Clear();
            await HubService.StopConnectionAsync();
            return ResultSuccess(true, "Conexión detenida");
        }

        public virtual async Task<IResultResponse<bool>> UpdateCollection()
        {
            var result = await GetAllAsync();
            if (!result.Success || result.Entity == null)
                return ResultError<bool>(result.Message, result.Error);

            Collection.ReplaceWith(result.Entity);

            return ResultSuccess(true, "Colección actualizada");
        }

        public async Task<IResultResponse<IEnumerable<TEntity>>> GetAllAsync()
        {
            var request = await GetRequest(HttpMethod.Get, BaseUrl);
            return await HandleResponseAsync<IEnumerable<TEntity>>(request, "Consulta exitosa");
        }

        public async Task<IResultResponse<TEntity>> GetByIdAsync(string id)
        {
            var request = await GetRequest(HttpMethod.Get, $"{BaseUrl}/{id}");
            return await HandleResponseAsync<TEntity>(request, "Consulta exitosa");
        }

        public async Task<IResultResponse<bool>> CreateAsync(TEntity entity)
        {
            var request = await GetRequest(HttpMethod.Post, BaseUrl, entity);
            return await HandleResponseAsync<bool>(request, "Creado exitosamente", true);
        }

        public async Task<IResultResponse<bool>> DeleteAsync(string id)
        {
            var request = await GetRequest(HttpMethod.Delete, $"{BaseUrl}/{id}");
            return await HandleResponseAsync<bool>(request, "Eliminado exitosamente", true);
        }

        public async Task<IResultResponse<bool>> UpdateAsync(string id, TEntity entity)
        {
            var request = await GetRequest(HttpMethod.Put, $"{BaseUrl}/{id}", entity);
            return await HandleResponseAsync<bool>(request, "Actualizado correctamente", true);
        }

        protected async Task<IResultResponse<T>> HandleResponseAsync<T>(HttpRequestMessage request, string successMessage, bool isVoid = false)
        {
            var client = GetClient();
            var response = await client.SendAsync(request /*.ConfigureAwait(false) */);

            if (!response.IsSuccessStatusCode)
                return await HandleError<T>(response);

            if (isVoid)
                return ResultSuccess<T>(default!, successMessage);

            return await JsonHelper.TryDeserializeAsync<T>(response, successMessage);
        }

        private ResultResponse<T> ResultSuccess<T>(T entity, string message) =>
            new() { Success = true, Entity = entity, Message = message };

        private ResultResponse<T> ResultError<T>(string message, string error = "") =>
            new() { Success = false, Entity = default!, Message = message, Error = error };

        public async Task<ResultResponse<T>> HandleError<T>(HttpResponseMessage response)
        {
            string content = await response.Content.ReadAsStringAsync();
            try
            {
                var error = JsonConvert.DeserializeObject<ErrorResponse>(content);
                return ResultError<T>(error?.Message ?? "Error inesperado", error?.Error ?? "");
            }
            catch (Exception ex)
            {
                return ResultError<T>($"Error desconocido. \n {ex.Message}", content);
            }
        }
    }


    public class ResultResponse<TObj> : IResultResponse<TObj>
    {
        public TObj Entity { get; init; } = default!;
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }
}
