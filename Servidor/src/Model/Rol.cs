﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Shared.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Shared.Interfaces.Model.Obj;
using Shared.Interfaces.Model;

namespace Servidor.src.Model
{
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
            throw new System.NotImplementedException();
        }

        public void AddOrRemove(string id)
        {

        }

        public bool VerView { get; set; }

    }
}
