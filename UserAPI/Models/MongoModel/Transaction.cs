// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace UserAPI.Models.MongoModel
{
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string categoryId { get; set; }

        public string walletId { get; set; }

        public double amount { get; set; }

        public DateTime createAt { get; set; }

        public string note { get; set; }
    }

    public class NewTransactionInfo
    {
        [Required(ErrorMessage = "categoryId is required")]
        public string categoryId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Please enter amount")]
        public double amount { get; set; }

        public string note { get; set; }
    }
}
