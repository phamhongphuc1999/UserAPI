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

namespace UserAPI.Configuration
{
    public class JWTConfig
    {
        public int ExpireMinutes { get; set; }
        public string SecretKey { get; set; }
    }

    public class MongoConfig
    {
        public string Connect { get; set; }
        public string Database { get; set; }
    }

    public class SQLConfig
    {
        public string Connect { get; set; }
        public string[] EmployeeFields { get; set; }
    }

    public class SQLiteConfig
    {
        public string Connect { get; set; }
    }

    public class DevelopmentConfig
    {
        public string ApplicationUrl { get; set; }
        public string Version { get; set; }
        public string ApplicationName { get; set; }
    }

    public class ProductionConfig
    {
        public string ApplicationUrl { get; set; }
        public string Version { get; set; }
        public string ApplicationName { get; set; }
    }
}
