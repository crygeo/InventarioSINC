using System.IO;
using System.IO.IsolatedStorage;
using System.Net.Http;
using Newtonsoft.Json;
using Shared.Response;
using Utilidades.Interfaces;

namespace Cliente.Services;

public class AuthService : HttpClientBase
{
    public override string BaseUrl { get; } = $"{Config.FullUrl}/auth"; // Ajusta la URL de tu API


    public async Task<IResultResponse<TokenResponse>> LoginAsync(string username, string password)
    {
        var loginData = new { User = username, Password = password };

        var reques = await GetRequest<AuthService>(HttpMethod.Post, $"{BaseUrl}/login", loginData);
        var resut = await HandleResponseAsync<TokenResponse, AuthService>(reques, "Seccion Iniciado Exitosamente.", false);
        if (resut.Success && !string.IsNullOrEmpty(resut.EntityGet.Token))
            await SetToken(resut.EntityGet.Token);
        return resut;
    }
    public async Task<TimeSpan> VerificarLogin()
    {
        try
        {
            var client = GetClient();
            var reques = await GetRequest<AuthService>(HttpMethod.Get, $"{BaseUrl}/validate-token");

            var response = await client.SendAsync(reques);

            if (!response.IsSuccessStatusCode)
                return TimeSpan.Zero; // Token inválido o sesión expirada

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var tokenData = JsonConvert.DeserializeObject<TokenValidationResponse>(jsonResponse) ?? new TokenValidationResponse();

            if (tokenData.TimeRemaining <= 0)
                return TimeSpan.Zero; // Expirado

            // Convertir el tiempo restante a hh:mm:ss
            return TimeSpan.FromSeconds(tokenData.TimeRemaining);
        }
        catch (TokenException)
        {
            return TimeSpan.Zero;  // No hay sesión activa
        }

    }
    public async Task<bool> VerificarServidorAsync()
    {
        try
        {
            Console.WriteLine($"Intento de coneccion a {BaseUrl}");
            var client = GetClient();
            var reques = await GetRequest<AuthService>(HttpMethod.Get, $"{BaseUrl}/check");

            var response = await client.SendAsync(reques);
            response.EnsureSuccessStatusCode();
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

    public static async Task GuardarCredenciales(string username, string password, bool remember)
    {
        using var storage = IsolatedStorageFile.GetUserStoreForApplication();
        using var stream = new IsolatedStorageFileStream("credentials.txt", FileMode.Create, storage);
        using var writer = new StreamWriter(stream);

        await writer.WriteLineAsync(username);
        await writer.WriteLineAsync(password);
        await writer.WriteLineAsync(remember.ToString());
    }
    public static async Task BorrarCredenciales()
    {
        await Task.Run(() =>
        {
            using var storage = IsolatedStorageFile.GetUserStoreForApplication();
            if (storage.FileExists("credentials.txt"))
                storage.DeleteFile("credentials.txt");
        });
    }
    public static async Task<(string, string, bool)> CargarCredenciales()
    {
        using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
        {
            if (storage.FileExists("credentials.txt"))
            {
                using var stream = new IsolatedStorageFileStream("credentials.txt", FileMode.Open, storage);
                using var reader = new StreamReader(stream);

                string username = await reader.ReadLineAsync() ?? string.Empty;
                string password = await reader.ReadLineAsync() ?? string.Empty;
                string rememberString = await reader.ReadLineAsync() ?? string.Empty;

                // Convertir el valor "remember" a bool, si no es válido asignamos "false"
                bool remember = bool.TryParse(rememberString, out bool result) && result;

                return (username, password, remember);
            }
        }

        return (string.Empty, string.Empty, false); // Si no existe el archivo, devuelves el valor por defecto
    }

    public class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
    }


}