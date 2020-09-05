using System.Collections.Generic;

namespace UserAPI.Services
{
    public static class Config
    {
        public static Dictionary<string, string> employeeFields = new Dictionary<string, string>()
        {
            {"Username", "0" },
            {"Password", "0" },
            {"Name", "0" },
            {"Image", "0" },
            {"Birthday", "0" },
            {"Sex", "0" },
            {"Phone", "0" },
            {"Address", "0" },
            {"Position", "0" },
            {"Node", "0" }
        };
    }
}
