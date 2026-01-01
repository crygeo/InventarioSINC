using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Attributes;
using Shared.Interfaces.Model;
using Shared.Interfaces.Model.Obj;

namespace Servidor.Model;

[AutoController]
public class Rol : IRol
{
    private bool _verView;

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Nombre { get; set; }
    public List<string> Permisos { get; set; }
    public bool IsAdmin { get; set; }
    public bool Deleteable { get; set; }
    public bool Updatable { get; set; }

    public void Update(IModelObj entity)
    {
        throw new NotImplementedException();
    }
}