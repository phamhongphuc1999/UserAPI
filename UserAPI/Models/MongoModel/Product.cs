﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace UserAPI.Models.MongoModel
{
  public class Product
  {
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string id { get; set; }

    public string userId { get; set; }

    public string name { get; set; }

    public double price { get; set; }

    public DateTime createAt { get; set; }

    public DateTime updateAt { get; set; }
  }

  public class InsertProduct
  {
    public string name { get; set; }
    public double price { get; set; }
  }
}
