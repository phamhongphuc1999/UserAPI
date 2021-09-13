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

using MongoDB.Driver;
using static UserAPI.Program;

namespace UserAPI.Services.MongoService.MongoDataService
{
    public class BaseDataService<T>
    {
        protected IMongoCollection<T> mCollection;

        public BaseDataService(string collection)
        {
            mCollection = APIConnecter.Mongo.MDatabase.GetCollection<T>(collection);
        }
    }
}
