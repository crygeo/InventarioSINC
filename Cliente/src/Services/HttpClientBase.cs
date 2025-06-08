using System.IO;
using System.IO.IsolatedStorage;
using System.Net.Http;
using System.Text;
using Cliente.Extencions;
using Cliente.Services.Model;
using Newtonsoft.Json;
using Shared.ObjectsResponse;
using Utilidades.Interfaces;

namespace Cliente.Services;

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

    protected async Task<HttpRequesMessage<T>> GetRequest<T>()
    {
        var token = await GetToken();
        var reques = new HttpRequesMessage<T>();
        reques.Headers.Add("Authorization", $"Bearer {token}");


        return reques;
    }

    protected async Task<HttpRequesMessage<T>> GetRequest<T>(string Uri)
    {
        var reques = await GetRequest<T>();
        reques.RequestUri = new Uri(Uri);
        return reques;
    }

    protected async Task<HttpRequesMessage<T>> GetRequest<T>(HttpMethod Meth, string Uri)
    {
        var reques = await GetRequest<T>(Uri);
        reques.Method = Meth;
        return reques;
    }

    protected async Task<HttpRequesMessage<T>> GetRequest<T>(HttpMethod Meth, string Uri, object content)
    {
        var jsonContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

        var reques = await GetRequest<T>(Meth, Uri);
        reques.Content = jsonContent;
        return reques;
    }

    protected async Task<IResultResponse<T>> HandleResponseAsync<T, TE>(HttpRequesMessage<TE> request, string successMessage, bool isVoid = false)
    {
        var client = GetClient();
        var response = await client.SendAsync(request /*.ConfigureAwait(false) */);
        if (!response.IsSuccessStatusCode)
            return await HandleError<T>(response);

        IResultResponse<T> result;
        if (isVoid)
            result = ResultSuccess<T>(default!, successMessage);
        else
            result = await JsonHelper.TryDeserializeAsync<T>(response, successMessage);
            
        result.Method = request.Method;
        result.ObjInteration = typeof(TE);
        return result;
    }

    protected async Task<ResultResponse<T>> HandleError<T>(HttpResponseMessage response)
    {
        string content = await response.Content.ReadAsStringAsync();

        try
        {
            // 1. Intentar deserializar a tu ErrorResponse personalizado
            var error = JsonConvert.DeserializeObject<ErrorResponse>(content);
            if (!string.IsNullOrWhiteSpace(error?.Message))
            {
                return ResultError<T>(error.Message, error.Error);
            }

            // 2. Intentar deserializar como error de validación de ASP.NET Core
            var validationError = JsonConvert.DeserializeObject<ValidationProblemDetails>(content);
            if (validationError?.Errors != null && validationError.Errors.Any())
            {
                var formattedErrors = string.Join("\n", validationError.Errors.SelectMany(e =>
                    e.Value.Select(msg => $"{e.Key}: {msg}")
                ));

                return ResultError<T>(validationError.Title ?? "Error de validación", formattedErrors);
            }

            // 3. Si no se reconoce el formato
            return ResultError<T>("Error inesperado", content);
        }
        catch (Exception ex)
        {
            return ResultError<T>($"Error desconocido.\n{ex.Message}", content);
        }
    }


    protected ResultResponse<T> ResultSuccess<T>(T entity, string message) =>
        new() { Success = true, EntityGet = entity, Message = message};

    protected ResultResponse<T> ResultError<T>(string message, string error = "") =>
        new() { Success = false, EntityGet = default!, Message = message, Error = error };
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

public class ValidationProblemDetails
{
    public string? Title { get; set; }
    public int? Status { get; set; }
    public Dictionary<string, string[]> Errors { get; set; } = new();
}