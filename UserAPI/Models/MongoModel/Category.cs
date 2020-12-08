// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace UserAPI.Models.MongoModel
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string iconId { get; set; }

        public string name { get; set; }

        public string parentCategory { get; set; }

        public string typeCategory { get; set; }
    }

    public class NewCategoryInfo
    {
        [Required]
        public string name { get; set; }

        [Required]
        public string iconId { get; set; }

        public string parentCategory { get; set; }

        [IncludeArray(CheckArray = new string[4] { "Loan", "Expense", "Income", "Deat" })]
        public string typeCategory { get; set; }
    }
}
