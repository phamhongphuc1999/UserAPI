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
using System;
using System.Data.SQLite;
using UserAPI.Configuration;

namespace UserAPI.Connecter
{
    public class SQLiteConnecter
    {
        private static SQLiteConnecter connecter;
        public SQLiteConfig Config { get; private set; }

        public SQLiteConnection connection {
            get; private set;
        }

        private SQLiteConnecter(IConfigurationSection configuration)
        {
            Config = new SQLiteConfig();
            configuration.Bind(Config);
            string currentPath = Environment.CurrentDirectory;
            string connectString = string.Format("Data Source={0}; Version = 3;", currentPath + Config.Connect);
            connection = new SQLiteConnection();
            connection.ConnectionString = connectString;
        }

        public static SQLiteConnecter GetInstance(IConfigurationSection configuration)
        {
            if (connecter == null) connecter = new SQLiteConnecter(configuration);
            return connecter;
        }

        public void OpenConnection()
        {
            connection.Open();
        }

        public void CloseConnection()
        {
            connection.Close();
        }
    }
}
