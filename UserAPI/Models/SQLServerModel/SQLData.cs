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

namespace UserAPI.Models.SQLServerModel
{
    public class SQLData : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public SQLData(DbContextOptions<SQLData> options) : base(options)
        {
        }
    }
}
