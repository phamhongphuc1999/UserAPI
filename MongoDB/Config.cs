using System.Collections.Generic;

namespace MongoDB
{
    public static class Config
    {
        public static string MONGO_SCRIPT = "mongodb://localhost:27017";

        public static Dictionary<string, string> userStatus = new Dictionary<string, string>()
        {
            {"enable", "0" },
            {"disable", "0" }
        };

        public static Dictionary<string, int> userRole = new Dictionary<string, int>()
        {
            {"admin", 0 },
            {"user", 1 }
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
    }
}
