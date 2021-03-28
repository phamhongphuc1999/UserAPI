// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using System;
using UserAPI.Models.MongoModel;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.Models.CommonModel;
using MongoDB.Bson;

namespace UserAPI.Services.MongoService
{
    public class UserService : BaseService<BsonDocument>
    {
        public UserService(string collection) : base(collection) { }

        public Result Login(string username, string password)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("username", username);
            BsonDocument user = mCollection.Find(filter).First();
            if (user == null) return new Result
            {
                status = 401,
                data = "username or password wrong"
            };
            string rawPassword = Helper.CalcuteSHA256Hash(password);
            BsonValue _password = user.GetElement("password").Value;
            if (_password.AsString != rawPassword) return new Result
            {
                status = 401,
                data = "username or password wrong"
            };
            bool status = user.GetElement("status").Value.AsBoolean;
            if (!status) return new Result
            {
                status = 403,
                data = "This account is enable to login"
            };
            UpdateDefinition<BsonDocument> updateBuilder = Builders<BsonDocument>.Update.Set("lastLogin", BsonDateTime.Create(DateTime.Now));
            mCollection.UpdateOne(filter, updateBuilder);
            return new Result
            {
                status = 200,
                data = user
            };
        }

        public async Task<Result> LoginAsync(string username, string password)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("username", username);
            BsonDocument user = await mCollection.Find(filter).FirstAsync();
            if (user == null) return new Result
            {
                status = 401,
                data = "username or password wrong"
            };
            string rawPassword = Helper.CalcuteSHA256Hash(password);
            BsonValue _password = user.GetElement("password").Value;
            if (_password.AsString != rawPassword) return new Result
            {
                status = 401,
                data = "username or password wrong"
            };
            bool status = user.GetElement("status").Value.AsBoolean;
            if (!status) return new Result
            {
                status = 403,
                data = "This account is enable to login"
            };
            UpdateDefinition<BsonDocument> updateBuilder = Builders<BsonDocument>.Update.Set("lastLogin", BsonDateTime.Create(DateTime.Now));
            mCollection.UpdateOne(filter, updateBuilder);
            return new Result
            {
                status = 200,
                data = user
            };
        }

        public Result Register(NewUserInfo entity)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("username", entity.username);
            BsonDocument user = mCollection.Find(filter).First();
            if (user != null) return new Result
            {
                status = 400,
                data = $"username {entity.username} have existed"
            };
            BsonDocument newUser = new BsonDocument
            {
                { "username", entity.username},
                { "password", Helper.CalcuteSHA256Hash(entity.password) },
                { "email", entity.email },
                { "createAt", DateTime.Now },
                { "updateAt", DateTime.Now },
                { "lastLogin", DateTime.Now },
                { "status", true }
            };
            mCollection.InsertOne(newUser);
            return new Result
            {
                status = 200,
                data = newUser
            };
        }

        public async Task<Result> RegisterAsync(NewUserInfo entity)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("username", entity.username);
            BsonDocument user = await mCollection.Find(filter).FirstOrDefaultAsync();
            if (user != null) return new Result
            {
                status = 400,
                data = $"username {entity.username} have existed"
            };
            BsonDocument newUser = new BsonDocument
            {
                { "username", entity.username},
                { "password", Helper.CalcuteSHA256Hash(entity.password) },
                { "email", entity.email },
                { "createAt", DateTime.Now },
                { "updateAt", DateTime.Now },
                { "lastLogin", DateTime.Now },
                { "status", true }
            };
            await mCollection.InsertOneAsync(newUser);
            return new Result
            {
                status = 200,
                data = newUser.ToJson()
            };
        }

        public Result GetUserById(string userId, string[] fields = null)
        {
            BsonDocument user;
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("id", userId);
            if (fields == null) user = mCollection.Find(filter).First();
            else
            {
                ProjectionDefinition<BsonDocument> projection = Builders<BsonDocument>.Projection.Include(fields[0]);
                int len = fields.Length;
                for (int i = 1; i < len; i++) projection = projection.Include(fields[i]);
                user = mCollection.Find(filter).Project(projection).First();
            }
            if (user == null) return new Result
            {
                status = 400,
                data = $"the user with id: {userId} do not exist"
            };
            return new Result
            {
                status = 200,
                data = user
            };
        }

        public async Task<Result> GetUserByIdAsync(string userId, string[] fields = null)
        {
            BsonDocument user;
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("id", userId);
            if (fields == null) user = await mCollection.Find(filter).FirstAsync();
            else
            {
                ProjectionDefinition<BsonDocument> projection = Builders<BsonDocument>.Projection.Include(fields[0]);
                int len = fields.Length;
                for (int i = 1; i < len; i++) projection = projection.Include(fields[i]);
                user = await mCollection.Find(filter).Project(projection).FirstAsync();
            }
            if (user == null) return new Result
            {
                status = 400,
                data = $"the user with id: {userId} do not exist"
            };
            return new Result
            {
                status = 200,
                data = user
            };
        }

        public Result GetUserByUserName(string username)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("username", username);
            BsonDocument user = mCollection.Find(filter).First();
            if (user == null) return new Result
            {
                status = 400,
                data = $"user with username: ${username} do not exist"
            };
            return new Result
            {
                status = 200,
                data = user
            };
        }

        public async Task<Result> GetUserByUserNameAsync(string username)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("username", username);
            BsonDocument user = await mCollection.Find(filter).FirstAsync();
            if (user == null) return new Result
            {
                status = 400,
                data = $"user with username: ${username} do not exist"
            };
            return new Result
            {
                status = 200,
                data = user
            };
        }

        public Result GetListUser(int pageSize = 0, int pageIndex = 0, string[] fields = null)
        {
            List<BsonDocument> userList = mCollection.Find(new BsonDocument()).ToList();
            int totalResult = userList.Count;
            if (pageSize == 0) pageSize = totalResult;
            if (pageIndex == 0) pageIndex = 1;
            int index = pageSize * (pageIndex - 1);
            return new Result
            {
                status = 200,
                data = new
                {
                    user_list = userList.GetRange(index, pageSize),
                    pagination = new
                    {
                        totalResult = totalResult,
                        pageIndex = pageIndex,
                        pageSize = pageSize
                    }
                }
            };
        }

        public async Task<Result> GetListUserAsync(int pageSize = 0, int pageIndex = 0, string[] fields = null)
        {
            List<BsonDocument> userList = await mCollection.Find(new BsonDocument()).ToListAsync();
            int totalResult = userList.Count;
            if (pageSize == 0) pageSize = totalResult;
            if (pageIndex == 0) pageIndex = 1;
            int index = pageSize * (pageIndex - 1);
            return new Result
            {
                status = 200,
                data = new
                {
                    user_list = userList.GetRange(index, pageSize),
                    pagination = new
                    {
                        totalResult = totalResult,
                        pageIndex = pageIndex,
                        pageSize = pageSize
                    }
                }
            };
        }

        public Result UpdateUser(string userId, UpdateUserInfo updateUser)
        {
            UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set("updateAt", BsonDateTime.Create(DateTime.Now));
            if (updateUser.username != null)
            {
                FilterDefinition<BsonDocument> nameFilter = Builders<BsonDocument>.Filter.Eq("username", updateUser.username);
                BsonDocument checkUser = mCollection.Find(nameFilter).First();
                if (checkUser != null) return new Result
                {
                    status = 400,
                    data = $"the username: {updateUser.username} is exist"
                };
                update = update.Set("username", updateUser.username);
            }
            if (updateUser.password != null)
            {
                string newPassword = Helper.CalcuteSHA256Hash(updateUser.password);
                update = update.Set("password", newPassword);
            }
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("id", userId);
            UpdateResult result = mCollection.UpdateOne(filter, update);
            if (result.ModifiedCount > 0) return new Result
            {
                status = 200,
                data = result
            };
            else return new Result
            {
                status = 400,
                data = $"do not update user with userId: {userId}"
            };
        }

        public async Task<Result> UpdateUserAsync(string userId, UpdateUserInfo updateUser)
        {
            UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set("updateAt", BsonDateTime.Create(DateTime.Now));
            if (updateUser.username != null)
            {
                FilterDefinition<BsonDocument> nameFilter = Builders<BsonDocument>.Filter.Eq("username", updateUser.username);
                BsonDocument checkUser = await mCollection.Find(nameFilter).FirstAsync();
                if (checkUser != null) return new Result
                {
                    status = 400,
                    data = $"the username: {updateUser.username} is exist"
                };
                update = update.Set("username", updateUser.username);
            }
            if (updateUser.password != null)
            {
                string newPassword = Helper.CalcuteSHA256Hash(updateUser.password);
                update = update.Set("password", newPassword);
            }
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("id", userId);
            UpdateResult result = mCollection.UpdateOne(filter, update);
            if (result.ModifiedCount > 0) return new Result
            {
                status = 200,
                data = result
            };
            else return new Result
            {
                status = 400,
                data = $"do not update user with userId: {userId}"
            };
        }

        public Result DeleteUser(string username)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("username", username);
            DeleteResult result = mCollection.DeleteOne(filter);
            if (result.DeletedCount > 0) return new Result
            {
                status = 200,
                data = result
            };
            else return new Result
            {
                status = 400,
                data = $"do not delete user with id: {username}"
            };
        }

        public async Task<Result> DeleteUserAsync(string username)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("username", username);
            DeleteResult result = await mCollection.DeleteOneAsync(filter);
            if (result.DeletedCount > 0) return new Result
            {
                status = 200,
                data = result
            };
            else return new Result
            {
                status = 400,
                data = $"do not delete user with id: {username}"
            };
        }
    }
}
