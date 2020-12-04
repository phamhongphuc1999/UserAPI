// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

namespace UserAPI.Services.MongoService
{
    public static class Config
    {
        public static string MONGO_SCRIPT = "mongodb://localhost:27017";

        #region User Config
        public static string[] USER_FIELDS = new string[]
        {
            "_id", "username", "pasword", "email", "createAt", "updateAt", "lastLogin", "status"
        };
        #endregion
    }
}
