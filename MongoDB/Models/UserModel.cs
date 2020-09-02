using MongoDB.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Secrets;

namespace MongoDB.Models
{
    public class UserModel: BaseModel<User>
    {
        public UserModel(): base()
        {
            mCollection = client.GetDatabase("User");
            mDocument = mCollection.GetCollection<User>("user_list");
        }

        public async Task<Result> Login(string username, string password)
        {
            User user = mDocument.Find(x => x.username == username).ToList().FirstOrDefault();
            if (user == null) return new Result
            {
                status = 401,
                data = $"username or password wrong"
            };
            string rawPassword = SHA256Hash.CalcuteHash(password);
            if (user.password != rawPassword) return new Result
            {
                status = 401,
                data = $"username or password wrong"
            };
            if (user.status == "disable") return new Result
            {
                status = 403,
                data = "This account is enable to login"
            };
            UpdateDefinition<User> updateBuilder = Builders<User>.Update.Set(x => x.lastLogin, Hepler.CurrentTime());
            await mDocument.FindOneAndUpdateAsync(x => x.username == username, updateBuilder);
            return new Result
            {
                status = 200,
                data = user.role
            };
        }

        public async Task<Result> InsertUser(User entity)
        {
            User user = mDocument.Find(x => x.username == entity.username).ToList().FirstOrDefault();
            if (user != null) return new Result
            {
                status = 400,
                data = $"username {entity.username} have existed"
            };
            entity.password = SHA256Hash.CalcuteHash(entity.password);
            entity.createAt = Hepler.CurrentTime();
            entity.updateAt = Hepler.CurrentTime();
            entity.status = "enable";
            await mDocument.InsertOneAsync(entity);
            User newUser = mDocument.Find(x => x.username == entity.username).ToList().FirstOrDefault();
            return new Result
            {
                status = 200,
                data = newUser
            };
        }

        public async Task<Result> GetUserById(string userId, string[] fields = null)
        {
            List<User> result = await mDocument.Find(x => x._id == userId).ToListAsync();
            User user = result.FirstOrDefault();
            if (user == null) return new Result
            {
                status = 400,
                data = $"the user with id: {userId} do not exist"
            };
            if (fields == null) return new Result
            {
                status = 200,
                data = user
            };
            BsonDocument sUser = user.ToBsonDocument();
            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach (string field in fields)
                if (Config.userFields.ContainsKey(field))
                    data.Add(field, sUser.GetElement(field).Value.ToString());
            return new Result
            {
                status = 200,
                data = data
            };
        }

        public async Task<Result> GetListUser()
        {
            List<User> userList = await mDocument.Find(x => x.name != String.Empty).ToListAsync();
            return new Result
            {
                status = 200,
                data = userList
            };
        }

        public async Task<Result> UpdateUser(string userId, User updateUser)
        {
            UpdateDefinition<User> updateBuilder = Builders<User>.Update.Set(x => x.updateAt, Hepler.CurrentTime());
            if (updateUser.name != null) updateBuilder = updateBuilder.Set(x => x.name, updateUser.name);
            if (updateUser.location != null) updateBuilder = updateBuilder.Set(x => x.location, updateUser.location);
            if (updateUser.birthday != null) updateBuilder = updateBuilder.Set(x => x.birthday, updateUser.birthday);
            if (updateUser.phone != null) updateBuilder = updateBuilder.Set(x => x.phone, updateUser.phone);
            if (updateUser.username != null)
            {
                User checkUser = mDocument.Find(x => x.username == updateUser.username).ToList().FirstOrDefault();
                if (checkUser != null) return new Result
                {
                    status = 400,
                    data = $"the username: {updateUser.username} is exist"
                };
                updateBuilder = updateBuilder.Set(x => x.username, updateUser.username);
            }
            if (updateUser.password != null)
            {
                string newPassword = SHA256Hash.CalcuteHash(updateUser.password);
                updateBuilder = updateBuilder.Set(x => x.password, newPassword);
            }
            User user = await mDocument.FindOneAndUpdateAsync(x => x._id == userId, updateBuilder);
            if (user != null) return new Result
            {
                status = 200,
                data = user
            };
            else return new Result
            {
                status = 400,
                data = $"do not update user with id: {userId}"
            };
        }

        public async Task<Result> UpdateRole(string userId, User updateRoleUser)
        {
            UpdateDefinition<User> updateBuilder = Builders<User>.Update.Set(x => x.updateAt, Hepler.CurrentTime());
            if (updateRoleUser.role != null) updateBuilder.Set(x => x.role, updateRoleUser.role);
            if (updateRoleUser.status != null)
            {
                if (Config.userStatus.ContainsKey(updateRoleUser.status))
                    updateBuilder.Set(x => x.status, updateRoleUser.status);
                else return new Result
                {
                    status = 422,
                    data = $"Invalid value status: {updateRoleUser.status}"
                };
            }
            User user = await mDocument.FindOneAndUpdateAsync(x => x._id == userId, updateBuilder);
            if (user != null) return new Result
            {
                status = 200,
                data = user
            };
            else return new Result
            {
                status = 400,
                data = $"do not update role user with id: {userId}"
            };
        }

        public async Task<Result> DeleteUser(string userId)
        {
            User user = await mDocument.FindOneAndDeleteAsync(x => x._id == userId);
            if (user != null) return new Result
            {
                status = 200,
                data = user
            };
            else return new Result
            {
                status = 400,
                data = $"do not delete user with id: {userId}"
            };
        }
    }
}
