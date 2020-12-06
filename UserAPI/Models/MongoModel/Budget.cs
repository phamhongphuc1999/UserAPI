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
    public class Budget
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public MongoDBRef walletId { get; set; }

        public MongoDBRef categoryId { get; set; }

        public double amount { get; set; }

        public BsonDateTime dateFrom { get; set; }

        public BsonDateTime dateTo { get; set; }
    }

    public class NewBudgetInfo
    {
        [Required(ErrorMessage = "walletId is reuiqred")]
        public MongoDBRef walletId { get; set; }

        [Required(ErrorMessage = "expenseId is required")]
        public string categoryId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Please enter amount")]
        public double amount { get; set; }

        [Required(ErrorMessage = "dateFrom is required")]
        public string dateFrom { get; set; }

        [Required(ErrorMessage = "dateTo is required")]
        public string dateTo { get; set; }
    }
}
