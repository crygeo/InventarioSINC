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
using Shared.Interfaces.ModelsBase;
using Utilidades.Interfaces;

namespace Cliente.src.Services.Model
{
    public class ServiceBase<TEntity> : HttpClientBase, IServiceClient<TEntity>
    where TEntity : IModelObj
    {
        public readonly Utilidades.Interfaces.IHubService<TEntity> HubService;

        public ObservableCollection<TEntity> Collection => HubService.Collection;

        public override string BaseUrl { get; } = $"{Config.FullUrl}/{typeof(TEntity).Name}";

        public ServiceBase(Utilidades.Interfaces.IHubService<TEntity> hubService)
        {
            HubService = hubService;
        }

        public virtual async Task<IResultResponse<bool>> InitAsync()
        {
            await HubService.StartConnectionAsync();
            Collection.Clear();

            var result = await GetAllAsync();
            if (!result.Success || result.EntityGet == null)
                return ResultError<bool>(result.Message, result.Error);

            Collection.ReplaceWith(result.EntityGet);

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
            if (!result.Success || result.EntityGet == null)
                return ResultError<bool>(result.Message, result.Error);

            Collection.ReplaceWith(result.EntityGet);

            return ResultSuccess(true, "Colección actualizada");
        }

        public async Task<IResultResponse<IEnumerable<TEntity>>> GetAllAsync()
        {
            var request = await GetRequest<TEntity>(HttpMethod.Get, BaseUrl);
            return await HandleResponseAsync<IEnumerable<TEntity>, TEntity>(request, "Consulta exitosa");
        }

        public async Task<IResultResponse<TEntity>> GetByIdAsync(string id)
        {
            var request = await GetRequest<TEntity>(HttpMethod.Get, $"{BaseUrl}/{id}");
            return await HandleResponseAsync<TEntity, TEntity>(request, "Consulta exitosa");
        }

        public TEntity GetById(string id)
        {
            return Collection.First(a => a.Id == id);
        }

        public async Task<IResultResponse<bool>> CreateAsync(TEntity entity)
        {
            var request = await GetRequest<TEntity>(HttpMethod.Post, BaseUrl, entity);
            return await HandleResponseAsync<bool, TEntity>(request, "Creado exitosamente", true);
        }

        public async Task<IResultResponse<bool>> DeleteAsync(string id)
        {
            var request = await GetRequest<TEntity>(HttpMethod.Delete, $"{BaseUrl}/{id}");
            return await HandleResponseAsync<bool, TEntity>(request, "Eliminado exitosamente", true);
        }

        public async Task<IResultResponse<bool>> UpdateAsync(string id, TEntity entity)
        {
            var request = await GetRequest<TEntity>(HttpMethod.Put, $"{BaseUrl}/{id}", entity);
            return await HandleResponseAsync<bool, TEntity>(request, "Actualizado correctamente", true);
        }

        

       

       
    }


    public class ResultResponse<TObj> : IResultResponse<TObj>
    {
        public TObj EntityGet { get; init; } = default!;
        public Type? ObjInteration { get; set; }
        public HttpMethod? Method { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }
}
