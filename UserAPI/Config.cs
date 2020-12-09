// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

namespace UserAPI
{
    public class JWTConfig
    {
        public int ExpireMinutes { get; set; }
        public string SecretKey { get; set; }
    }

    public class MongoSetting
    {
        public string Connect { get; set; }
        public string Database { get; set; }
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
