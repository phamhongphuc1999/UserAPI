// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserAPI.Models.MongoModel;

namespace UserAPI.Data.MongoDataService
{
    public class UserDataService : BaseDataService<BsonDocument>
    {
        public UserDataService(string collection) : base(collection) { }

        public bool InsertUser(NewUserInfo entity)
        {
            try
            {
                FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("username", entity.username);
                BsonDocument user = mCollection.Find(filter).First();
                if (user != null) return false;
                BsonDocument newUser = new BsonDocument
                {
                    { "username", entity.username},
                    { "password", Utilities.CalcuteSHA256Hash(entity.password) },
                    { "email", entity.email },
                    { "createAt", DateTime.Now },
                    { "updateAt", DateTime.Now },
                    { "lastLogin", DateTime.Now },
                    { "status", true }
                };
                mCollection.InsertOne(newUser);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> InsertUserAsync(NewUserInfo entity)
        {
            try
            {
                FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("username", entity.username);
                BsonDocument user = await mCollection.Find(filter).FirstOrDefaultAsync();
                if (user != null) return false;
                BsonDocument newUser = new BsonDocument
                {
                    { "username", entity.username},
                    { "password", Utilities.CalcuteSHA256Hash(entity.password) },
                    { "email", entity.email },
                    { "createAt", DateTime.Now },
                    { "updateAt", DateTime.Now },
                    { "lastLogin", DateTime.Now },
                    { "status", true }
                };
                await mCollection.InsertOneAsync(newUser);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public BsonDocument GetSingleUser(FilterDefinition<BsonDocument> filter, string[] fields = null)
        {
            BsonDocument user;
            if (fields == null) user = mCollection.Find(filter).FirstOrDefault();
            else
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (string field in fields) dic.Add(field, 1);
                ProjectionDefinition<BsonDocument> projection = new BsonDocument(dic);
                user = mCollection.Find(filter).Project(projection).FirstOrDefault();
            }
            return user;
        }

        public async Task<BsonDocument> GetSingleUserAsync(FilterDefinition<BsonDocument> filter, string[] fields = null)
        {
            BsonDocument user;
            if (fields == null) user = await mCollection.Find(filter).FirstOrDefaultAsync();
            else
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (string field in fields) dic.Add(field, 1);
                ProjectionDefinition<BsonDocument> projection = new BsonDocument(dic);
                user = await mCollection.Find(filter).Project(projection).FirstOrDefaultAsync();
            }
            return user;
        }

        public List<BsonDocument> GetListUsers(FilterDefinition<BsonDocument> filter, string[] fields = null)
        {
            List<BsonDocument> userList;
            if (fields == null) userList = mCollection.Find(filter).ToList();
            else
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (string field in fields) dic.Add(field, 1);
                ProjectionDefinition<BsonDocument> projection = new BsonDocument(dic);
                userList = mCollection.Find(filter).Project(projection).ToList();
            }
            return userList;
        }

        public List<BsonDocument> GetListUsers(string[] fields = null)
        {
            return GetListUsers(new BsonDocument(), fields);
        }

        public async Task<List<BsonDocument>> GetListUsersAsync(FilterDefinition<BsonDocument> filter, string[] fields = null)
        {
            List<BsonDocument> userList = new List<BsonDocument>();
            if (userList == null) userList = await mCollection.Find(filter).ToListAsync();
            else
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (string field in fields) dic.Add(field, 1);
                ProjectionDefinition<BsonDocument> projection = new BsonDocument(dic);
                userList = await mCollection.Find(filter).Project(projection).ToListAsync();
            }
            return userList;
        }

        public async Task<List<BsonDocument>> GetListUsersAsync(string[] fields = null)
        {
            return await GetListUsersAsync(new BsonDocument(), fields);
        }

        public bool UpdateUser(FilterDefinition<BsonDocument> filter, UpdateUserInfo updateUser)
        {
            try
            {
                UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set("updateAt", BsonDateTime.Create(DateTime.Now));
                if (updateUser.username != null)
                {
                    FilterDefinition<BsonDocument> nameFilter = Builders<BsonDocument>.Filter.Eq("username", updateUser.username);
                    BsonDocument checkUser = mCollection.Find(nameFilter).First();
                    if (checkUser != null) return false;
                    update = update.Set("username", updateUser.username);
                }
                if (updateUser.password != null)
                {
                    string newPassword = Utilities.CalcuteSHA256Hash(updateUser.password);
                    update = update.Set("password", newPassword);
                }
                if (updateUser.email != null) update = update.Set("email", updateUser.email);
                UpdateResult result = mCollection.UpdateOne(filter, update);
                return result.ModifiedCount > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateUserAsync(FilterDefinition<BsonDocument> filter, UpdateUserInfo updateUser)
        {
            try
            {
                UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set("updateAt", BsonDateTime.Create(DateTime.Now));
                if (updateUser.username != null)
                {
                    FilterDefinition<BsonDocument> nameFilter = Builders<BsonDocument>.Filter.Eq("username", updateUser.username);
                    BsonDocument checkUser = await mCollection.Find(nameFilter).FirstAsync();
                    if (checkUser != null) return false;
                    update = update.Set("username", updateUser.username);
                }
                if (updateUser.password != null)
                {
                    string newPassword = Utilities.CalcuteSHA256Hash(updateUser.password);
                    update = update.Set("password", newPassword);
                }
                if (updateUser.email != null) update = update.Set("email", updateUser.email);
                UpdateResult result = mCollection.UpdateOne(filter, update);
                return result.ModifiedCount > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteUser(string userId)
        {
            try
            {
                FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", userId);
                DeleteResult result = mCollection.DeleteOne(filter);
                return result.DeletedCount > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", userId);
                DeleteResult result = await mCollection.DeleteOneAsync(filter);
                return result.DeletedCount > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
