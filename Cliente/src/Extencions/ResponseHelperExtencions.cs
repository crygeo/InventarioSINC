using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades.Interfaces;

namespace Cliente.src.Extencions
{
    public static class ResponseHelperExtencions
    {
        public static async Task<bool> Validar <T>(this IResultResponse<T> resp, Func<Task> onError)
        {
            if (resp.Success) return true;
            await onError();
            return false;
        }
    }
}
