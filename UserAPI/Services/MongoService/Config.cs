// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

namespace UserAPI.Services.MongoService
{
    public static class Config
    {
        //public static string MONGO_SCRIPT = "mongodb://localhost:27017";
        public static string MONGO_SCRIPT = "mongodb+srv://admin:adminAirquality@cluster0.twdop.mongodb.net/MoneyLover?authSource=admin&replicaSet=atlas-my635u-shard-0&w=majority&readPreference=primary&appname=MongoDB%20Compass&retryWrites=true&ssl=true";

        #region User Config
        public static string[] USER_FIELDS = new string[]
        {
            "_id", "username", "pasword", "email", "createAt", "updateAt", "lastLogin", "status"
        };
        #endregion
    }
}
