using System.Net.Http;
using Cliente.Services.Model;
using Newtonsoft.Json;
using Utilidades.Interfaces;

namespace Cliente.Extencions;

public static class JsonHelper
{
    public static IResultResponse<T> TryDeserialize<T>(string json, string successMessage = "Deserialización exitosa")
    {
        try
        {
            var obj = JsonConvert.DeserializeObject<T>(json);

            if (obj == null)
            {
                return new ResultResponse<T>
                {
                    Success = false,
                    EntityGet = default!,
                    Message = "No se pudo deserializar el objeto.",
                    Error = "El contenido fue nulo o incompatible con el tipo esperado."
                };
            }

            return new ResultResponse<T>
            {
                Success = true,
                EntityGet = obj,
                Message = successMessage,
                Error = ""
            };
        }
        catch (Exception ex)
        {
            return new ResultResponse<T>
            {
                Success = false,
                EntityGet = default!,
                Message = "Error al procesar la respuesta.",
                Error = $"Excepción durante la deserialización: {ex.Message}"
            };
        }
    }

    public static async Task<IResultResponse<T>> TryDeserializeAsync<T>(HttpResponseMessage response, string successMessage = "Deserialización exitosa")
    {
        try
        {
            var tu = typeof(T);
            var json = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<T>(json);

            if (obj == null)
            {
                return new ResultResponse<T>
                {
                    Success = false,
                    EntityGet = default!,
                    Message = "No se pudo deserializar el objeto.",
                    Error = "El contenido fue nulo o incompatible con el tipo esperado.",
                };
            }

            return new ResultResponse<T>
            {
                Success = true,
                EntityGet = obj,
                Message = successMessage,
                Error = "",
            };
        }
        catch (Exception ex)
        {
            return new ResultResponse<T>
            {
                Success = false,
                EntityGet = default!,
                Message = "Error al procesar la respuesta.",
                Error = $"Excepción durante la deserialización: {ex.Message}",

            };
        }
    }



}