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

using Microsoft.Extensions.Configuration;

namespace UserAPI.Connecter
{
    public class APIConnection
    {
        public MongoConnecter Mongo { get; private set; }
        public SQLConnecter SQL { get; private set; }
        public SQLiteConnecter SQLite { get; private set; }

        /// <summary>
        /// The constructor of APIConnection class
        /// </summary>
        /// <param name="mongoConfig">The config of mongo connecter</param>
        /// <param name="sqlConfig">The config of sql server connecter</param>
        /// <param name="sqliteConfig">The config of sqlite connecter</param>
        public APIConnection(IConfigurationSection mongoConfig, IConfigurationSection sqlConfig, IConfigurationSection sqliteConfig)
        {
            Mongo = MongoConnecter.GetInstance(mongoConfig);
            SQL = SQLConnecter.GetInstance(sqlConfig);
            SQLite = SQLiteConnecter.GetInstance(sqliteConfig);
            SQLite.OpenConnection();
        }
    }
}
