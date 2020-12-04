// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;

namespace UserAPI.Models.MongoModel
{
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public MongoDBRef expenseId { get; set; }

        public MongoDBRef walletId { get; set; }

        public double amount { get; set; }

        public DateTime date { get; set; }

        public string note { get; set; }
    }
}
