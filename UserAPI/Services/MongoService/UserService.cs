// -------------------- SIMPLE API -------------------- 
//
//
// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
//
//
// Product by: Pham Hong Phuc
//
//
// ----------------------------------------------------

using System;
using UserAPI.Models.MongoModel;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserAPI.Models.CommonModel;
using MongoDB.Bson;
using UserAPI.Contances;
using UserAPI.Services.MongoService.MongoDataService;

namespace UserAPI.Services.MongoService
{
    public class UserService
    {
        private UserDataService service;

        public UserService(string collection)
        {
            service = new UserDataService(collection);
        }

        public Result Login(string username, string password)
        {
            FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter = builder.Eq("username", username);
            BsonDocument user = service.GetSingleUser(filter);
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
            bool check = service.UpdateUser(filter, new UpdateUserInfo { lastLogin = DateTime.Now });
            if (check) return new Result
            {
                status = Status.OK,
                data = new HeplerTokenUser
                {
                    userId = user.GetElement("_id").Value.ToString(),
                    username = user.GetElement("username").Value.AsString,
                    email = user.GetElement("email").Value.AsString
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
            FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter = builder.Eq("username", username);
            BsonDocument user = await service.GetSingleUserAsync(filter);
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
            bool check = await service.UpdateUserAsync(filter, new UpdateUserInfo { lastLogin = DateTime.Now });
            if (check) return new Result
            {
                status = Status.OK,
                data = new HeplerTokenUser
                {
                    userId = user.GetElement("_id").Value.ToString(),
                    username = user.GetElement("username").Value.AsString,
                    email = user.GetElement("email").Value.AsString
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
            FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter = builder.Eq("_id", ObjectId.Parse(userId));
            BsonDocument user = service.GetSingleUser(filter, fields);
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
            FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter = builder.Eq("_id", ObjectId.Parse(userId));
            BsonDocument user = await service.GetSingleUserAsync(filter, fields);
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
            FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter = builder.Eq("username", username);
            BsonDocument user = service.GetSingleUser(filter, fields);
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
            FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter = builder.Eq("username", username);
            BsonDocument user = await service.GetSingleUserAsync(filter, fields);
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
            FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter = builder.Eq("_id", userId);
            bool result = service.UpdateUser(filter, updateUser);
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
            FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter = builder.Eq("_id", userId);
            bool result = await service.UpdateUserAsync(filter, updateUser);
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
