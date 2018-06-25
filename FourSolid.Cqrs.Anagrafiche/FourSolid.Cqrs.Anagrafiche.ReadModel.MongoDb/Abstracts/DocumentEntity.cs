﻿using MongoDB.Bson.Serialization.Attributes;

namespace FourSolid.Cqrs.Anagrafiche.ReadModel.MongoDb.Abstracts
{
    public abstract class DocumentEntity : IDocumentEntity
    {
        [BsonId]
        public string Id { get; set; }
        public bool IsDeleted { get; protected set; }
    }
}