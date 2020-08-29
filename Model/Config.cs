using System.Collections.Generic;

namespace Model
{
    public static class Config
    {
        public static string MONGO_SCRIPT = "mongodb://localhost:27017";
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
            {"role", "0" },
            {"createAt", "0" },
            {"updateAt", "0" },
            {"lastLogin", "0" },
            {"status", "0" }
        };
    }
}
