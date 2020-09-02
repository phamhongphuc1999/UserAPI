using System.Collections.Generic;

namespace MongoDatabase
{
    public static class Config
    {
        public static string MONGO_SCRIPT = "mongodb://localhost:27017";

        #region User Config
        public static Dictionary<string, string> userStatus = new Dictionary<string, string>()
        {
            {"enable", "0" },
            {"disable", "0" }
        };

        public static Dictionary<string, int> userRole = new Dictionary<string, int>()
        {
            {"admin", 0 },
            {"user", 1 },
            {"customer", 2 }
        };

        public static Dictionary<string, string> userFields = new Dictionary<string, string>()
        {
            {"_id", "0" },
            {"name", "0" },
            {"username", "0" },
            {"password", "0" },
            {"location", "0" },
            {"birthday", "0" },
            {"email", "0" },
            {"phone", "0" },
            {"role", "1" },
            {"createAt", "0" },
            {"updateAt", "0" },
            {"lastLogin", "0" },
            {"status", "1" }
        };
        #endregion

        #region Product Config
        public static Dictionary<string, string> productStatus = new Dictionary<string, string>()
        {
            {"enable", "0" },
            {"disable", "0" }
        };

        public static Dictionary<string, string> productFields = new Dictionary<string, string>()
        {
            {"_id", "0" },
            {"name", "0" },
            {"origin", "0" },
            {"amount", "0" },
            {"price", "0" },
            {"guarantee", "0" },
            {"sale", "0" },
            {"createAt", "0" },
            {"updateAt", "0" },
            {"status", "0" }
        };
        #endregion
    }
}
