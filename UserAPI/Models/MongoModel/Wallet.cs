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
    public class Wallet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public MongoDBRef userId { get; set; }

        public MongoDBRef currencyId { get; set; }

        public MongoDBRef iconId { get; set; }

        public string name { get; set; }

        public double amount { get; set; }
    }

    public class NewWalletInfo
    {
        [Required(ErrorMessage = "userId is required")]
        public MongoDBRef userId { get; set; }

        [Required(ErrorMessage = "currencyId is required")]
        public MongoDBRef currencyId { get; set; }

        [Required(ErrorMessage = "iconId is required")]
        public MongoDBRef iconId { get; set; }

        [Required(ErrorMessage = "the username is required", AllowEmptyStrings = false)]
        [StringLength(200)]
        public string name { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Please enter amount")]
        public double amount { get; set; }
    }
}
