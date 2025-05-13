using System;
using System.IO.IsolatedStorage;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Net.Http.Headers;

namespace InventarioSINCliente.src.Services
{

    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private const string API_URL = "https://localhost:7226/api/Auth"; // Ajusta la URL de tu API

        public AuthService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var loginData = new { User = username, Password = password };

            var jsonContent = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

            var client = GetClient();

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(responseBody);
                string token = jsonDoc.RootElement.GetProperty("token").GetString() ?? string.Empty;

                if (!string.IsNullOrEmpty(token))
                    return await SaveTokenAsync(token);
            }

            return false; // Login fallido
        }
        public async Task<TimeSpan> VerificarLogin()
        {
            string token = await GetToken();
            if (string.IsNullOrEmpty(token))
                return TimeSpan.Zero; // No hay sesión activa

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.GetAsync($"{API_URL}/validate-token");

            if (!response.IsSuccessStatusCode)
                return TimeSpan.Zero; // Token inválido o sesión expirada

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var tokenData = JsonSerializer.Deserialize<TokenValidationResponse>(jsonResponse) ?? new TokenValidationResponse();

            if (tokenData.timeRemaining <= 0)
                return TimeSpan.Zero; // Expirado

            // Convertir el tiempo restante a hh:mm:ss
            return TimeSpan.FromSeconds(tokenData.timeRemaining); 
        }
        public async Task GuardarCredenciales(string username, string password, bool remember)
        {
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var stream = new IsolatedStorageFileStream("credentials.txt", FileMode.Create, storage))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        await writer.WriteLineAsync(username);
                        await writer.WriteLineAsync(password);
                        await writer.WriteLineAsync(remember.ToString());
                    }
                }
            }
        }
        public async Task BorrarCredenciales()
        {
            await Task.Run(() =>
            {
                using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (storage.FileExists("credentials.txt"))
                        storage.DeleteFile("credentials.txt");
                }
            });
        }
        public async Task<bool> VerificarServidorAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7226/api/health/check");
                return response.IsSuccessStatusCode; // Devuelve true si la API responde correctamente
            }
            catch (HttpRequestException)
            {
                return false; // Error en la conexión con el servidor
            }
            catch (TaskCanceledException)
            {
                return false; // Timeout, el servidor no respondió a tiempo
            }
            catch (Exception)
            {
                return false; // Otro tipo de error
            }
        }
        public async Task<(string, string, bool)> CargarCredenciales()
        {
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storage.FileExists("credentials.txt"))
                {
                    using (var stream = new IsolatedStorageFileStream("credentials.txt", FileMode.Open, storage))
                    using (var reader = new StreamReader(stream))
                    {
                        string username = await reader.ReadLineAsync() ?? string.Empty;
                        string password = await reader.ReadLineAsync() ?? string.Empty;
                        string rememberString = await reader.ReadLineAsync() ?? string.Empty;

                        // Convertir el valor "remember" a bool, si no es válido asignamos "false"
                        bool remember = bool.TryParse(rememberString, out bool result) ? result : false;

                        return (username, password, remember);
                    }
                }
            }

            return (string.Empty, string.Empty, false); // Si no existe el archivo, devuelves el valor por defecto
        }

        private async Task<bool> SaveTokenAsync(string token)
        {
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // Abrir el archivo de forma asíncrona
                using (var stream = new IsolatedStorageFileStream("auth_token.txt", FileMode.Create, storage))
                {
                    // Usar StreamWriter asíncrono para escribir el token
                    using (var writer = new StreamWriter(stream))
                    {
                        await writer.WriteAsync(token);
                        return true;// Guardas el token de forma asíncrona
                    }
                }
            }
        }
        private async Task<string> GetToken()
        {
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storage.FileExists("auth_token.txt"))
                {
                    using (var stream = new IsolatedStorageFileStream("auth_token.txt", FileMode.Open, storage))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            return await reader.ReadToEndAsync();
                        }
                    }
                }
            }
            return "";

        }
        public void DeleteToken()
        {
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storage.FileExists("auth_token.txt"))
                {
                    storage.DeleteFile("auth_token.txt");
                }
            }
        }

    }
    public class TokenValidationResponse
    {
        public string message { get; set; }
        public string user { get; set; }
        public double timeRemaining { get; set; } // Tiempo en segundos
    }
}
