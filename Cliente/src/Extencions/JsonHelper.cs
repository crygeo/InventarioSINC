using Cliente.src.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cliente.src.Services.Model;
using Utilidades.Interfaces;

namespace Cliente.src.Extencions
{
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

}
