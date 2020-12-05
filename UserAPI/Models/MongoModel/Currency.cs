using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;

namespace UserAPI.Models.MongoModel
{
    public class Currency
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string name { get; set; }

        public MongoDBRef iconId { get; set; }
    }

    public class NewCurrencyInfo
    {
        [Required(ErrorMessage = "name is required")]
        public string name { get; set; }

        [Required(ErrorMessage = "iconId is required")]
        public string iconId { get; set; }
    }
}
