// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using UserAPI.Models.MongoModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.Models.CommonModel;
using MongoDB.Bson;
using Microsoft.Extensions.Options;

namespace UserAPI.Services.MongoService
{
    public class UserService : BaseService<User>
    {
        public UserService(IOptions<MongoSetting> options, string collection) : base(options)
        {
            mCollection = mDatabase.GetCollection<User>(collection);
        }

        public Result Login(string username, string password)
        {
            User user = mCollection.Find(x => x.username == username).FirstOrDefault();
            if (user == null) return new Result
            {
                status = 401,
                data = "username or password wrong"
            };
            string rawPassword = HelperService.CalcuteSHA256Hash(password);
            if (user.password != rawPassword) return new Result
            {
                status = 401,
                data = "username or password wrong"
            };
            if (!user.status) return new Result
            {
                status = 403,
                data = "This account is enable to login"
            };
            UpdateDefinition<User> updateBuilder = Builders<User>.Update.Set(x => x.lastLogin, BsonDateTime.Create(DateTime.Now));
            mCollection.FindOneAndUpdate(x => x.username == username, updateBuilder);
            return new Result
            {
                status = 200,
                data = ""
            };
        }

        public async Task<Result> LoginAsync(string username, string password)
        {
            User user = await mCollection.Find(x => x.username == username).FirstOrDefaultAsync();
            if (user == null) return new Result
            {
                status = 401,
                data = "username or password wrong"
            };
            string rawPassword = HelperService.CalcuteSHA256Hash(password);
            if (user.password != rawPassword) return new Result
            {
                status = 401,
                data = "username or password wrong"
            };
            if (!user.status) return new Result
            {
                status = 403,
                data = "This account is enable to login"
            };
            UpdateDefinition<User> updateBuilder = Builders<User>.Update.Set(x => x.lastLogin, BsonDateTime.Create(DateTime.Now));
            await mCollection.FindOneAndUpdateAsync(x => x.username == username, updateBuilder);
            return new Result
            {
                status = 200,
                data = ""
            };
        }

        public Result InsertUser(NewUserInfo entity)
        {
            User user = mCollection.Find(x => x.username == entity.username).ToList().FirstOrDefault();
            if (user != null) return new Result
            {
                status = 400,
                data = $"username {entity.username} have existed"
            };
            User newUser = new User()
            {
                username = entity.username,
                password = HelperService.CalcuteSHA256Hash(entity.password),
                email = entity.email,
                createAt = DateTime.Now,
                updateAt = DateTime.Now,
                lastLogin = DateTime.Now,
                status = true
            };
            mCollection.InsertOne(newUser);
            return new Result
            {
                status = 200,
                data = newUser
            };
        }

        public async Task<Result> InsertUserAsync(NewUserInfo entity)
        {
            User user = mCollection.Find(x => x.username == entity.username).ToList().FirstOrDefault();
            if (user != null) return new Result
            {
                status = 400,
                data = $"username {entity.username} have existed"
            };
            User newUser = new User()
            {
                username = entity.username,
                password = HelperService.CalcuteSHA256Hash(entity.password),
                email = entity.email,
                createAt = DateTime.Now,
                updateAt = DateTime.Now,
                lastLogin = DateTime.Now,
                status = true
            };
            await mCollection.InsertOneAsync(newUser);
            return new Result
            {
                status = 200,
                data = newUser
            };
        }

        public Result GetUserById(string userId, string[] fields = null)
        {
            List<User> result = mCollection.Find(x => x._id == userId).ToList();
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
            Dictionary<string, object> data = new Dictionary<string, object>();
            foreach (string field in fields)
                if (_options.Value.UserFields.Contains(field))
                    data.Add(field, result.GetType().GetProperty(field).GetValue(result));
            return new Result
            {
                status = 200,
                data = data
            };
        }

        public async Task<Result> GetUserByIdAsync(string userId, string[] fields = null)
        {
            List<User> result = await mCollection.Find(x => x._id == userId).ToListAsync();
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
            Dictionary<string, object> data = new Dictionary<string, object>();
            foreach (string field in fields)
                if (_options.Value.UserFields.Contains(field))
                    data.Add(field, result.GetType().GetProperty(field).GetValue(result));
            return new Result
            {
                status = 200,
                data = data
            };
        }

        public Result GetUserByUserName(string username)
        {
            User user = mCollection.Find(x => x.username == username).ToList().FirstOrDefault();
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
            List<User> users = await mCollection.Find(x => x.username == username).ToListAsync();
            User user = users.FirstOrDefault();
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
            List<User> userList = mCollection.Find(x => x.username != String.Empty).ToList();
            int totalResult = userList.Count;
            if (pageSize == 0) pageSize = totalResult;
            if (pageIndex == 0) pageIndex = 1;
            int index = pageSize * (pageIndex - 1);
            if (fields == null) return new Result
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
            List<User> tempList = userList.GetRange(index, pageSize);
            IEnumerable<Dictionary<string, object>> userFilterList = tempList.Select(e =>
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (string field in fields)
                {
                    object value = e.GetType().GetProperty(field).GetValue(e);
                    result.Add(field, value);
                }
                return result;
            });
            return new Result
            {
                status = 200,
                data = new
                {
                    user_list = userFilterList,
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
            List<User> userList = await mCollection.Find(x => x.username != String.Empty).ToListAsync();
            int totalResult = userList.Count;
            if (pageSize == 0) pageSize = totalResult;
            if (pageIndex == 0) pageIndex = 1;
            int index = pageSize * (pageIndex - 1);
            if (fields == null) return new Result
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
            List<User> tempList = userList.GetRange(index, pageSize);
            IEnumerable<Dictionary<string, object>> userFilterList = tempList.Select(e =>
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (string field in fields)
                {
                    object value = e.GetType().GetProperty(field).GetValue(e);
                    result.Add(field, value);
                }
                return result;
            });
            return new Result
            {
                status = 200,
                data = new
                {
                    user_list = userFilterList,
                    pagination = new
                    {
                        totalResult = totalResult,
                        pageIndex = pageIndex,
                        pageSize = pageSize
                    }
                }
            };
        }

        public Result UpdateUser(string username, UpdateUserInfo updateUser)
        {
            UpdateDefinition<User> updateBuilder = Builders<User>.Update.Set(x => x.updateAt, BsonDateTime.Create(DateTime.Now));
            if (updateUser.username != null)
            {
                User checkUser = mCollection.Find(x => x.username == updateUser.username).ToList().FirstOrDefault();
                if (checkUser != null) return new Result
                {
                    status = 400,
                    data = $"the username: {updateUser.username} is exist"
                };
                updateBuilder = updateBuilder.Set(x => x.username, updateUser.username);
            }
            if (updateUser.password != null)
            {
                string newPassword = HelperService.CalcuteSHA256Hash(updateUser.password);
                updateBuilder = updateBuilder.Set(x => x.password, newPassword);
            }
            User user = mCollection.FindOneAndUpdate(x => x.username == username, updateBuilder);
            if (user != null)
            {
                user = mCollection.Find(x => x._id == user._id).FirstOrDefault();
                return new Result
                {
                    status = 200,
                    data = user
                };
            }
            else return new Result
            {
                status = 400,
                data = $"do not update user with username: {username}"
            };
        }

        public async Task<Result> UpdateUserAsync(string username, UpdateUserInfo updateUser)
        {
            UpdateDefinition<User> updateBuilder = Builders<User>.Update.Set(x => x.updateAt, BsonDateTime.Create(DateTime.Now));
            if (updateUser.username != null)
            {
                User checkUser = mCollection.Find(x => x.username == updateUser.username).ToList().FirstOrDefault();
                if (checkUser != null) return new Result
                {
                    status = 400,
                    data = $"the username: {updateUser.username} is exist"
                };
                updateBuilder = updateBuilder.Set(x => x.username, updateUser.username);
            }
            if (updateUser.password != null)
            {
                string newPassword = HelperService.CalcuteSHA256Hash(updateUser.password);
                updateBuilder = updateBuilder.Set(x => x.password, newPassword);
            }
            User user = await mCollection.FindOneAndUpdateAsync(x => x.username == username, updateBuilder);
            if (user != null)
            {
                user = mCollection.Find(x => x._id == user._id).FirstOrDefault();
                return new Result
                {
                    status = 200,
                    data = user
                };
            }
            else return new Result
            {
                status = 400,
                data = $"do not update user with username: {username}"
            };
        }

        public Result DeleteUser(string username)
        {
            User user = mCollection.FindOneAndDelete(x => x.username == username);
            if (user != null) return new Result
            {
                status = 200,
                data = user
            };
            else return new Result
            {
                status = 400,
                data = $"do not delete user with id: {username}"
            };
        }

        public async Task<Result> DeleteUserAsync(string username)
        {
            User user = await mCollection.FindOneAndDeleteAsync(x => x.username == username);
            if (user != null) return new Result
            {
                status = 200,
                data = user
            };
            else return new Result
            {
                status = 400,
                data = $"do not delete user with id: {username}"
            };
        }
    }
}
