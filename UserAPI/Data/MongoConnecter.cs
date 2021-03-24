// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace UserAPI.Data
{
    public class MongoConnecter
    {
        public MongoClient Client { get; private set; }
        public IMongoDatabase MDatabase { get; private set; }
        public MongoSetting Setting { get; private set; }

        private static MongoConnecter connecter;

        private MongoConnecter(IConfigurationSection configuration)
        {
            Setting = new MongoSetting();
            configuration.Bind(Setting);
            Client = new MongoClient(Setting.Connect);
            MDatabase = Client.GetDatabase(Setting.Database);
        }

        public static MongoConnecter GetInstance(IConfigurationSection configuration)
        {
            if (connecter == null) connecter = new MongoConnecter(configuration);
            return connecter;
        }
    }
}
