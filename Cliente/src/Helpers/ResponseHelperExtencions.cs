using Utilidades.Interfaces;

namespace Cliente.Helpers;

public static class ResponseHelperExtencions
{
    public static async Task<bool> Validar<T>(this IResultResponse<T> resp, Func<Task> onError)
    {
        if (resp.Success) return true;
        await onError();
        return false;
    }
}