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
using System.IO;
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

        /// <summary>
        /// Constructor of MongoConnecter
        /// </summary>
        /// <param name="configuration">Contain all of config for connecter</param>
        private SQLiteConnecter(IConfigurationSection configuration)
        {
            Config = new SQLiteConfig();
            configuration.Bind(Config);
            string currentPath = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(currentPath).FullName;
            string connectString = string.Format("Data Source={0}; Version = 3;", projectDirectory + Config.Connect);
            connection = new SQLiteConnection();
            connection.ConnectionString = connectString;
        }

        /// <summary>
        /// Return single instance of connecter
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns>The instance of connecter</returns>
        public static SQLiteConnecter GetInstance(IConfigurationSection configuration)
        {
            if (connecter == null) connecter = new SQLiteConnecter(configuration);
            return connecter;
        }

        /// <summary>
        /// Open the connection
        /// </summary>
        public void OpenConnection()
        {
            connection.Open();
        }

        /// <summary>
        /// Close the connection
        /// </summary>
        public void CloseConnection()
        {
            connection.Close();
        }
    }
}
