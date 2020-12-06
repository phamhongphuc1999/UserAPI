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
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public MongoDBRef iconId { get; set; }

        public string name { get; set; }
    }

    public class NewCategoryInfo
    {
        [Required(ErrorMessage = "iconId is required")]
        public string iconId { get; set; }

        [Required(ErrorMessage = "name is required")]
        public string name { get; set; }
    }
}
