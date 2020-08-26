using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model
{
    public class mongodb
    {
        MongoClient client;
        IMongoDatabase userDatabase;
        IMongoCollection<User> users;
        public mongodb()
        {
            client = new MongoClient("mongodb://localhost:27017");
            userDatabase = client.GetDatabase("User");
            users = userDatabase.GetCollection<User>("user_list");
        }

        public async Task<List<User>> GetListUser()
        {
            List<User> userList = await users.Find(x => x.name != String.Empty).ToListAsync();
            return userList;
        }

        public async Task<bool> InsertUser()
        {
            await users.InsertOneAsync(new User()
            {
                name = "abc",
                username = "abc",
                password = "abc",
                birthday = "123"
            });
            return true;
        }
    }
}
