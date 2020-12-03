// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserAPI.Models.SQLServerModel;

namespace UserAPI.Services.SQLServerService
{
    public class BaseService
    {
        private SQLData sqlData;
        private DbContextOptionsBuilder<SQLData> option;

        public BaseService(IConfiguration configuration)
        {
            option = new DbContextOptionsBuilder<SQLData>();
            option.UseSqlServer(configuration.GetConnectionString("SQLServer"));
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
    }
}
