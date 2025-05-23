using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ObjectsResponse
{
    public class ErrorResponse
    {
        public string Message { get; init; } = string.Empty;
        public string Error { get; init; } = string.Empty;  
        public int StatusCode { get; init; }
        public ErrorResponse(int statusCode, string message, string? error = null)
        {
            StatusCode = statusCode;
            Message = message;
            Error = error ?? "";
        }

        public ErrorResponse(string message, string? error = null)
        {
            Message = message;
            Error = error ?? "";
        }

        public ErrorResponse() { }
    }
}
