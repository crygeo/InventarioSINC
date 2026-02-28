using System;
using System.Net.Http;
using Utilidades.Interfaces;

namespace Utilidades.Objs;

public class ResultResponse<TObj> : IResultResponse<TObj>, IResultResponse
{
    public string GetErrorFormat()
    {
        var nombreObjeto = ObjInteration?.Name ?? "Desconocido";

        var detalles = string.IsNullOrWhiteSpace(Error)
            ? "Sin detalles disponibles."
            : string.Join("\n\t", Error.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));

        return $"Entity relacionado: {nombreObjeto}\n\nDetalles del error:\n\t{detalles}";
    }

    public TObj EntityGet { get; init; } = default!;
    public Type? ObjInteration { get; set; }

    public HttpMethod? Method { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;

    public override string ToString()
    {
        return GetErrorFormat();
    }
}