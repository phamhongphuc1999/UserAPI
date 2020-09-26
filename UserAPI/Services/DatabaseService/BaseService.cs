﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserAPI.Models.SQLServer;

namespace UserAPI.Services.DatabaseService
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