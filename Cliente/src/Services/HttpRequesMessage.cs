using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.src.Services
{
    public class HttpRequesMessage<T> : HttpRequestMessage
    {
        public Type Type { get; set; } = typeof(T);
    }
}
