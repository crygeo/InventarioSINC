using Newtonsoft.Json;

namespace Shared.Extensions;

public static class CloningExtensions
{
    public static T Clone<T>(this T source)
    {
        return JsonConvert.DeserializeObject<T>(
            JsonConvert.SerializeObject(source));
    }
}