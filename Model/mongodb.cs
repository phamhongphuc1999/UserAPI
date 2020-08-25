using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class mongodb
    {
        MongoClient client;
        IMongoDatabase userDatabase;
        IMongoCollection<BsonDocument> users;
        public mongodb()
        {
            client = new MongoClient("mongodb://localhost:27017");
            userDatabase = client.GetDatabase("User");
            users = userDatabase.GetCollection<BsonDocument>("UserList");
        }

        public List<BsonElement> GetListUser()
        {
            List<BsonElement> userList = users.ToBsonDocument().ToList();
            return userList;
        }
    }
}
