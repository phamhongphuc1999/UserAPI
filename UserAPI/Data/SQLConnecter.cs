// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserAPI.Models.SQLServerModel;

namespace UserAPI.Data
{
    public class SQLConnecter
    {
        public SQLData SqlData { get; private set; }
        public DbContextOptionsBuilder<SQLData> Option { get; private set; }
        public SQLSetting Setting { get; private set; }
        private static SQLConnecter connecter;

        private SQLConnecter(IConfigurationSection configuration)
        {
            Setting = new SQLSetting();
            configuration.Bind(Setting);
            Option = new DbContextOptionsBuilder<SQLData>();
            Option.UseSqlServer(Setting.Connect);
            SqlData = new SQLData(Option.Options);
        }

        public static SQLConnecter GetInstance(IConfigurationSection configuration)
        {
            if (connecter == null) connecter = new SQLConnecter(configuration);
            return connecter;
        }
    }
}
