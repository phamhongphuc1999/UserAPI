// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace UserAPI.Services.MongoService
{
    public class BaseService<T>
    {
        protected MongoClient client;
        protected IMongoDatabase mDatabase;
        protected IMongoCollection<T> mCollection;

        public BaseService(IOptions<MongoSetting> options)
        {
            client = new MongoClient(options.Value.Connect);
            mDatabase = client.GetDatabase(options.Value.Database);
        }
    }
}
