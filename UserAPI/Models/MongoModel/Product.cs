using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace UserAPI.Models.MongoModel
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string name { get; set; }

        public string origin { get; set; }

        public long amount { get; set; }

        public long price { get; set; }

        public int guarantee { get; set; }

        public int sale { get; set; }

        public string createAt { get; set; }

        public string updateAt { get; set; }

        public string status { get; set; }
    }

    public class NewProductInfo
    {
        [Required]
        public string name { get; set; }

        [Required]
        public string origin { get; set; }

        [Required]
        public long amount { get; set; }

        [Required]
        public long price { get; set; }

        public int guarantee { get; set; }

        public int sale { get; set; }
    }

    public class UpdateProductInfo
    {
        public string name { get; set; }

        public string origin { get; set; }

        public long amount { get; set; }

        public long price { get; set; }

        public int guarantee { get; set; }

        public int sale { get; set; }

        public string status { get; set; }
    }
}
