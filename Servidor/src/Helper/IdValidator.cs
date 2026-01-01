using MongoDB.Bson;

namespace Servidor.Helper;

public static class IdValidator
{
    public static bool IsValidObjectId(string id)
    {
        return ObjectId.TryParse(id, out _);
    }
}