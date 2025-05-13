using Cliente.src.ServicesHub;
using MongoDB.Bson;
using Newtonsoft.Json;
using Shared.Interfaces;
using Shared.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Utilidades.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Cliente.src.Services
{
    public abstract class ServiceBase<TItem> : HttpClientBase, IServiceClient<TItem> where TItem : IIdentifiable, ISelectable, IUpdate
    {
        public abstract HubServiceBase<TItem> HubService { get; }

        public ObservableCollection<TItem> Collection { get; } = [];


        public virtual async Task<(bool, string)> InitAsync()
        {
            await HubService.StartConnectionAsync(); // 🛑 Ahora solo se ejecuta cuando esté listo
            Collection.Clear();
            var collect = await GetAllAsync();
            foreach (var item in collect.Item1)
                Collection.Add(item);

            return (string.IsNullOrEmpty(collect.Item2), collect.Item2);
        }
        public virtual async Task StopASunc()
        {
            Collection.Clear();
            await HubService.StopConnectionAsync();
        }


        public async Task<(IEnumerable<TItem>, string)> GetAllAsync()
        {
            var client = GetClient();
            var request = await GetRequest(HttpMethod.Get, BaseUrl);
            var response = await client.SendAsync(request);

            var result = await ManejarErrores(response);
            if (result.Item1) return ([], result.Item2);

            var json = await response.Content.ReadAsStringAsync();
            var datos = JsonConvert.DeserializeObject<IEnumerable<TItem>>(json);
            return (datos ?? [], "");
        }


        public async Task<(TItem?, string)> GetByIdAsync(string id)
        {
            var client = GetClient();
            var request = await GetRequest(HttpMethod.Get, $"{BaseUrl}/{id}");
            var response = await client.SendAsync(request);

            var result = await ManejarErrores(response);
            if (result.Item1) return (default, result.Item2);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return (JsonConvert.DeserializeObject<TItem>(json), "");
        }
        public async Task<(bool, string)> CreateAsync(TItem entity)
        {
            var client = GetClient();
            var request = await GetRequest(HttpMethod.Post, BaseUrl, entity);
            var response = await client.SendAsync(request);

            var result = await ManejarErrores(response);
            return result.Item1 ? (false, result.Item2) : (true, "");
        }
        public async Task<(bool, string)> DeleteAsync(string id)
        {
            var client = GetClient();
            var request = await GetRequest(HttpMethod.Delete, $"{BaseUrl}/{id}");
            var response = await client.SendAsync(request);

            var result = await ManejarErrores(response);
            return result.Item1 ? (false, result.Item2) : (true, "");

        }
        public async Task<(bool, string)> UpdateAsync(string id, TItem entity)
        {
            var client = GetClient();
            var request = await GetRequest(HttpMethod.Put, $"{BaseUrl}/{id}", entity);
            var response = await client.SendAsync(request);

            var result = await ManejarErrores(response);
            return result.Item1? (false, result.Item2): (true, "") ;
        }

        public async Task<(bool, string)> ManejarErrores(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return (false, "");

            string message = await response.Content.ReadAsStringAsync();
           
            return (true,message);
        }



    }

    public class ErrorResponse
    {
        public string Error { get; set; }
        public string Message { get; set; }
    }
}
