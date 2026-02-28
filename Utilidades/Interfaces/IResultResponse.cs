using System;
using System.Net.Http;

namespace Utilidades.Interfaces;

public interface IResultResponse<TObj>
{
    public TObj EntityGet { get; init; }
    public Type? ObjInteration { get; set; }
    public HttpMethod Method { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
    public string Error { get; set; }
}

public interface IResultResponse
{
    string Error { get; }
    Type? ObjInteration { get; }
    string GetErrorFormat();
}