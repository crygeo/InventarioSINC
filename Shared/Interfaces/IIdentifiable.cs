using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Interfaces;

public interface IIdentifiable
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    string Id { get; set; }
}