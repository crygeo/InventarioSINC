using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Windows;
using Newtonsoft.Json;

namespace Cliente.src.Services
{
    public abstract class HttpClientBase
    {
        public abstract string BaseUrl { get; }
        private static string Token { get; set; } = string.Empty;

        public HttpClient GetClient()
        {
            return new HttpClient();
        }

        public static async Task<string> GetToken()
        {
            try
            {

                if (!string.IsNullOrEmpty(Token))
                    return Token;

                using var storage = IsolatedStorageFile.GetUserStoreForApplication();
                if (storage.FileExists("auth_token.txt"))
                {
                    using var stream = new IsolatedStorageFileStream("auth_token.txt", FileMode.Open, storage);
                    using var reader = new StreamReader(stream);
                    Token = await reader.ReadToEndAsync();
                }
            }
            catch { }

            return Token;

        }
        public static async Task<bool> SetToken(string token)
        {
            try
            {

                using var storage = IsolatedStorageFile.GetUserStoreForApplication();
                // Abrir el archivo de forma asíncrona
                using var stream = new IsolatedStorageFileStream("auth_token.txt", FileMode.Create, storage);
                // Usar StreamWriter asíncrono para escribir el token
                using var writer = new StreamWriter(stream);
                Token = token;
                await writer.WriteAsync(token);
                return true;
            }
            catch { return false; }
        }
        public static void DeleteToken()
        {
            using var storage = IsolatedStorageFile.GetUserStoreForApplication();
            if (storage.FileExists("auth_token.txt"))
            {
                storage.DeleteFile("auth_token.txt");
            }
            Token = string.Empty;
        }

        public async Task<HttpRequestMessage> GetRequest()
        {
            var token = await GetToken();
            var reques = new HttpRequestMessage();
            reques.Headers.Add("Authorization", $"Bearer {token}");


            return reques;
        }

        public async Task<HttpRequestMessage> GetRequest(string Uri)
        {
            var reques = await GetRequest();
            reques.RequestUri = new Uri(Uri);
            return reques;
        }

        public async Task<HttpRequestMessage> GetRequest(HttpMethod Meth, string Uri)
        {
            var reques = await GetRequest(Uri);
            reques.Method = Meth;
            return reques;
        }

        public async Task<HttpRequestMessage> GetRequest(HttpMethod Meth, string Uri, object content)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

            var reques = await GetRequest(Meth, Uri);
            reques.Content = jsonContent;
            return reques;
        }
    }


    public class TokenException : Exception
    {
        public TokenExceptions Exception { get; set; }
        public TokenException(TokenExceptions tokenExceptions)
        {
            Exception = tokenExceptions;
        }
    }
    public enum TokenExceptions
    {
        IsNull,
    }
}
