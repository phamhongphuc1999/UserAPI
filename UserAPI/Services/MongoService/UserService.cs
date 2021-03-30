// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using System;
using UserAPI.Models.MongoModel;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserAPI.Models.CommonModel;
using MongoDB.Bson;
using UserAPI.Contances;
using UserAPI.Data.MongoDataService;

namespace UserAPI.Services.MongoService
{
    public class UserService : BaseService<BsonDocument>
    {
        private UserDataService service;

        public UserService(string collection) : base(collection)
        {
            service = new UserDataService("Users");
        }

        public Result Login(string username, string password)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("username", username);
            BsonDocument user = service.GetUserByName(username);
            if (user == null) return new Result
            {
                status = Status.BadRequest,
                data = Messages.WrongUserPassword
            };
            string rawPassword = Utilities.CalcuteSHA256Hash(password);
            BsonValue _password = user.GetElement("password").Value;
            if (_password.AsString != rawPassword) return new Result
            {
                status = Status.Unauthorized,
                data = Messages.WrongUserPassword
            };
            bool status = user.GetElement("status").Value.AsBoolean;
            if (!status) return new Result
            {
                status = Status.Forbidden,
                data = Messages.EnableAccount
            };
            UpdateDefinition<BsonDocument> updateBuilder = Builders<BsonDocument>.Update.Set("lastLogin", BsonDateTime.Create(DateTime.Now));
            UpdateResult result = mCollection.UpdateOne(filter, updateBuilder);
            if (result.ModifiedCount > 0) return new Result
            {
                status = Status.OK,
                data = new
                {
                    id = user.GetElement("_id").Value.ToString(),
                    username = user.GetElement("username").Value.AsString
                }
            };
            return new Result
            {
                status = Status.BadRequest,
                data = Messages.BadRequest
            };
        }

        public async Task<Result> LoginAsync(string username, string password)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("username", username);
            BsonDocument user = await mCollection.Find(filter).FirstAsync();
            if (user == null) return new Result
            {
                status = Status.Unauthorized,
                data = Messages.WrongUserPassword
            };
            string rawPassword = Utilities.CalcuteSHA256Hash(password);
            BsonValue _password = user.GetElement("password").Value;
            if (_password.AsString != rawPassword) return new Result
            {
                status = Status.Unauthorized,
                data = Messages.WrongUserPassword
            };
            bool status = user.GetElement("status").Value.AsBoolean;
            if (!status) return new Result
            {
                status = Status.Forbidden,
                data = Messages.EnableAccount
            };
            UpdateDefinition<BsonDocument> updateBuilder = Builders<BsonDocument>.Update.Set("lastLogin", BsonDateTime.Create(DateTime.Now));
            UpdateResult result = await mCollection.UpdateOneAsync(filter, updateBuilder);
            if (result.ModifiedCount > 0) return new Result
            {
                status = Status.OK,
                data = new
                {
                    id = user.GetElement("_id").Value.ToString(),
                    username = user.GetElement("username").Value.AsString
                }
            };
            return new Result
            {
                status = Status.BadRequest,
                data = Messages.BadRequest
            };
        }

        public Result Register(NewUserInfo entity)
        {
            bool result = service.InsertUser(entity);
            if (!result) return new Result
            {
                status = Status.BadRequest,
                data = Messages.BadRequest
            };
            return new Result
            {
                status = Status.OK,
                data = Messages.OK
            };
        }

        public async Task<Result> RegisterAsync(NewUserInfo entity)
        {
            bool result = await service.InsertUserAsync(entity);
            if (!result) return new Result
            {
                status = Status.BadRequest,
                data = Messages.BadRequest
            };
            return new Result
            {
                status = Status.OK,
                data = Messages.OK
            };
        }

        public Result GetUserById(string userId, string[] fields = null)
        {
            BsonDocument user = service.GetUserById(userId, fields);
            if (user == null) return new Result
            {
                status = Status.BadRequest,
                data = Messages.NotExistedUser
            };
            return new Result
            {
                status = Status.OK,
                data = user.ToJson()
            };
        }

        public async Task<Result> GetUserByIdAsync(string userId, string[] fields = null)
        {
            BsonDocument user = await service.GetUserByIdAsync(userId, fields);
            if (user == null) return new Result
            {
                status = Status.BadRequest,
                data = Messages.NotExistedUser
            };
            return new Result
            {
                status = Status.OK,
                data = user.ToJson()
            };
        }

        public Result GetUserByUserName(string username, string[] fields = null)
        {
            BsonDocument user = service.GetUserByName(username, fields);
            if (user == null) return new Result
            {
                status = Status.BadRequest,
                data = Messages.NotExistedUser
            };
            return new Result
            {
                status = Status.OK,
                data = user.ToJson()
            };
        }

        public async Task<Result> GetUserByUserNameAsync(string username, string[] fields = null)
        {
            BsonDocument user = await service.GetUserByNameAsync(username, fields);
            if (user == null) return new Result
            {
                status = Status.BadRequest,
                data = Messages.NotExistedUser
            };
            return new Result
            {
                status = Status.OK,
                data = user.ToJson()
            };
        }

        public Result GetListUser(int pageSize = 0, int pageIndex = 0, string[] fields = null)
        {
            List<BsonDocument> userList = service.GetListUsers(fields);
            int totalResult = userList.Count;
            if (pageSize == 0) pageSize = totalResult;
            if (pageIndex == 0) pageIndex = 1;
            int index = pageSize * (pageIndex - 1);
            return new Result
            {
                status = Status.OK,
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
            List<BsonDocument> userList = await service.GetListUsersAsync(fields);
            int totalResult = userList.Count;
            if (pageSize == 0) pageSize = totalResult;
            if (pageIndex == 0) pageIndex = 1;
            int index = pageSize * (pageIndex - 1);
            return new Result
            {
                status = Status.OK,
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
            bool result = service.UpdateUser(userId, updateUser);
            if (result) return new Result
            {
                status = Status.OK,
                data = result
            };
            else return new Result
            {
                status = Status.BadRequest,
                data = Messages.BadRequest
            };
        }

        public async Task<Result> UpdateUserAsync(string userId, UpdateUserInfo updateUser)
        {
            bool result = await service.UpdateUserAsync(userId, updateUser);
            if (result) return new Result
            {
                status = Status.OK,
                data = result
            };
            else return new Result
            {
                status = Status.BadRequest,
                data = Messages.BadRequest
            };
        }

        public Result DeleteUser(string userId)
        {
            bool result = service.DeleteUser(userId);
            if (result) return new Result
            {
                status = Status.OK,
                data = result
            };
            else return new Result
            {
                status = Status.BadRequest,
                data = Messages.BadRequest
            };
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            bool result = await service.DeleteUserAsync(userId);
            if (result) return new Result
            {
                status = Status.OK,
                data = result
            };
            else return new Result
            {
                status = Status.BadRequest,
                data = Messages.BadRequest
            };
        }
    }
}
