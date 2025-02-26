using MongoDB.Bson;

namespace Servidor.src.Helper
{
    public static class IdValidator
    {
        public static bool IsValidObjectId(string id)
        {
            return ObjectId.TryParse(id, out _);
        }
    }

}
