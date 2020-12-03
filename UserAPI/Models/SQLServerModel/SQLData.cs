// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Microsoft.EntityFrameworkCore;

namespace UserAPI.Models.SQLServerModel
{
    public class SQLData: DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public SQLData(DbContextOptions<SQLData> options) : base(options)
        {
        }
    }
}
