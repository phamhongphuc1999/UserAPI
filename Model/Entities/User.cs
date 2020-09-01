using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Model
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string username { get; set; }

        public string password { internal get; set; }

        public string name { get; set; }

        public string location { get; set; }

        public string email { get; set; }

        public string birthday { get; set; }

        public string phone { get; set; }

        public string role { get; set; }

        public string createAt { get; set; }

        public string updateAt { get; set; }

        public string lastLogin { get; set; }

        public string status { get; set; }
    }
}
