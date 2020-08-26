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
            client = new MongoClient(Config.MONGO_SCRIPT);
            userDatabase = client.GetDatabase("User");
            users = userDatabase.GetCollection<User>("user_list");
        }

        public async Task<bool> InsertUser(User entity)
        {
            User user = users.Find(x => x.username == entity.username).ToList().FirstOrDefault();
            if (user != null) return false;
            entity.createAt = Hepler.CurrentTime();
            entity.updateAt = Hepler.CurrentTime();
            entity.status = "enable";
            await users.InsertOneAsync(entity);
            return true;
        }

        public async Task<User> GetUserById(string userId)
        {
            List<User> result = await users.Find(x => x._id == userId).ToListAsync();
            User user = result.FirstOrDefault();
            return user;
        }

        public async Task<List<User>> GetListUser()
        {
            List<User> userList = await users.Find(x => x.name != String.Empty).ToListAsync();
            return userList;
        }

        public async Task<bool> DeleteUSer(string userId)
        {
            User user = await users.FindOneAndDeleteAsync(x => x._id == userId);
            if (user != null) return true;
            else return false;
        }
    }
}
