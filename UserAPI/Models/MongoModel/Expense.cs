// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;

namespace UserAPI.Models.MongoModel
{
    public class Expense
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public MongoDBRef categoryId { get; set; }

        public MongoDBRef iconId { get; set; }

        public string name { get; set; }
    }

    public class NewExpenseInfo
    {
        [Required(ErrorMessage = "iconId is reequired")]
        public string iconId { get; set; }

        [Required(ErrorMessage = "name is reequired")]
        public string name { get; set; }
    }
}
