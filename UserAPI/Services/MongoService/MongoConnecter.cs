// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace UserAPI.Services.MongoService
{
    public class MongoConnecter
    {
        protected MongoClient client;
        protected IMongoDatabase mDatabase;
        protected MongoSetting setting;

        public MongoConnecter(IConfigurationSection configuration)
        {
            setting = new MongoSetting();
            configuration.Bind(setting);
            client = new MongoClient(setting.Connect);
            mDatabase = client.GetDatabase(setting.Database);
        }

        public IMongoDatabase MDatabase
        {
            get { return mDatabase; }
        }

        public MongoSetting Setting
        {
            get { return setting; }
        }
    }
}
