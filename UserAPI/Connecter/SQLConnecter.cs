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

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserAPI.Configuration;
using UserAPI.Models.SQLServerModel;

namespace UserAPI.Connecter
{
    public class SQLConnecter
    {
        public SQLData SqlData { get; private set; }
        public DbContextOptionsBuilder<SQLData> Option { get; private set; }
        public SQLConfig Config { get; private set; }
        private static SQLConnecter connecter;

        /// <summary>
        /// Constructor of MongoConnecter
        /// </summary>
        /// <param name="configuration">Contain all of config for connecter</param>
        private SQLConnecter(IConfigurationSection configuration)
        {
            Config = new SQLConfig();
            configuration.Bind(Config);
            Option = new DbContextOptionsBuilder<SQLData>();
            Option.UseSqlServer(Config.Connect);
            SqlData = new SQLData(Option.Options);
        }

        /// <summary>
        /// Return single instance of connecter
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns>The instance of connecter</returns>
        public static SQLConnecter GetInstance(IConfigurationSection configuration)
        {
            if (connecter == null) connecter = new SQLConnecter(configuration);
            return connecter;
        }
    }
}
