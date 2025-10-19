using System.Net.Http;

namespace Cliente.Services;

public class HttpRequesMessage<T> : HttpRequestMessage
{
    public Type Type { get; set; } = typeof(T);
}