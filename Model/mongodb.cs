using Model.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model.Secrets;

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

        //public async Task<Result> Login(string username, string password)
        //{
        //    User user = users.Find(x => x.username == username).ToList().FirstOrDefault();
        //    if (user != null) return new Result
        //    {
        //        status = false,
        //        data = $"username or password wrong"
        //    };
        //    string rawPassword = SHA256Hash.CalcuteHash(password);
        //    if(user.password != rawPassword) return new Result
        //    {
        //        status = false,
        //        data = $"username or password wrong"
        //    };
        //}

        public async Task<Result> InsertUser(User entity)
        {
            User user = users.Find(x => x.username == entity.username).ToList().FirstOrDefault();
            if (user != null) return new Result
            {
                status = false,
                data = $"username {entity.username} have existed"
            };
            entity.password = SHA256Hash.CalcuteHash(entity.password);
            entity.createAt = Hepler.CurrentTime();
            entity.updateAt = Hepler.CurrentTime();
            entity.status = "enable";
            await users.InsertOneAsync(entity);
            User newUser = users.Find(x => x.username == entity.username).ToList().FirstOrDefault();
            return new Result
            {
                status = true,
                data = newUser
            };
        }

        public async Task<Result> GetUserById(string userId, string[] fields = null)
        {
            List<User> result = await users.Find(x => x._id == userId).ToListAsync();
            User user = result.FirstOrDefault();
            if (user == null) return new Result
            {
                status = false,
                data = $"the user with id: {userId} do not exist"
            };
            if (fields == null) return new Result
            {
                status = true,
                data = user
            };
            BsonDocument sUser = user.ToBsonDocument();
            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach(string field in fields)
                if (Config.userFields.ContainsKey(field)) 
                    data.Add(field, sUser.GetElement(field).Value.ToString());
            return new Result
            {
                status = true,
                data = data
            };
        }

        public async Task<Result> GetListUser()
        {
            List<User> userList = await users.Find(x => x.name != String.Empty).ToListAsync();
            return new Result
            {
                status = true,
                data = userList
            };
        }

        public async Task<Result> UpdateUser(string userId, User updateUser)
        {
            updateUser.updateAt = Hepler.CurrentTime();
            UpdateDefinition<User> updateBuilder = Builders<User>.Update.Set(x => x.updateAt, Hepler.CurrentTime());
            if (updateUser.name != null) updateBuilder = updateBuilder.Set(x => x.name, updateUser.name);
            if (updateUser.location != null) updateBuilder = updateBuilder.Set(x => x.location, updateUser.location);
            if (updateUser.birthday != null) updateBuilder = updateBuilder.Set(x => x.birthday, updateUser.birthday);
            if (updateUser.phone != null) updateBuilder = updateBuilder.Set(x => x.phone, updateUser.phone);
            if (updateUser.username != null)
            {
                User checkUser = users.Find(x => x.username == updateUser.username).ToList().FirstOrDefault();
                if (checkUser != null) return new Result
                {
                    status = false,
                    data = $"the username: {updateUser.username} is exist"
                };
                updateBuilder = updateBuilder.Set(x => x.username, updateUser.username);
            }
            if (updateUser.password != null)
            {
                string newPassword = SHA256Hash.CalcuteHash(updateUser.password);
                updateBuilder = updateBuilder.Set(x => x.password, newPassword);
            }
            User user = await users.FindOneAndUpdateAsync(x => x._id == userId, updateBuilder);
            if (user != null) return new Result
            {
                status = true,
                data = user
            };
            else return new Result
            {
                status = false,
                data = $"do not update user with id: {userId}"
            };
        }

        public async Task<Result> DeleteUser(string userId)
        {
            User user = await users.FindOneAndDeleteAsync(x => x._id == userId);
            if (user != null) return new Result
            {
                status = true,
                data = user
            };
            else return new Result
            {
                status = false,
                data = $"do not delete user with id: {userId}"
            };
        }
    }
}
