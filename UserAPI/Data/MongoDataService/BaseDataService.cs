// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using MongoDB.Driver;
using static UserAPI.Program;

namespace UserAPI.Data.MongoDataService
{
    public class BaseDataService<T>
    {
        protected IMongoCollection<T> mCollection;

        public BaseDataService(string collection)
        {
            mCollection = mongoConnecter.MDatabase.GetCollection<T>(collection);
        }
    }
}
