﻿// -------------------- SIMPLE API -------------------- 
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

using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using UserAPI.Configuration;

namespace UserAPI.Connecter
{
    public class MongoConnecter
    {
        public MongoClient Client { get; private set; }
        public IMongoDatabase MDatabase { get; private set; }
        public MongoConfig Config { get; private set; }

        private static MongoConnecter connecter;

        private MongoConnecter(IConfigurationSection configuration)
        {
            Config = new MongoConfig();
            configuration.Bind(Config);
            Client = new MongoClient(Config.Connect);
            MDatabase = Client.GetDatabase(Config.Database);
        }

        public static MongoConnecter GetInstance(IConfigurationSection configuration)
        {
            if (connecter == null) connecter = new MongoConnecter(configuration);
            return connecter;
        }
    }
}