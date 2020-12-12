// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserAPI.Models.SQLServerModel;

namespace UserAPI.Services.SQLServerService
{
    public class SQLConnecter
    {
        protected SQLData sqlData;
        protected DbContextOptionsBuilder<SQLData> option;
        protected SQLSetting sqlSetting;

        public SQLConnecter(IConfigurationSection configuration)
        {
            sqlSetting = new SQLSetting();
            configuration.Bind(sqlSetting);
            option = new DbContextOptionsBuilder<SQLData>();
            option.UseSqlServer(sqlSetting.Connect);
            sqlData = new SQLData(option.Options);
        }

        public SQLData SqlData
        {
            get { return sqlData; }
        }

        public DbContextOptionsBuilder<SQLData> Option
        {
            get { return option; }
        }

        public SQLSetting Setting
        {
            get { return sqlSetting; }
        }
    }
}
