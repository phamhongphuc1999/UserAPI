using System;
using System.Security.Cryptography;
using System.Text;

namespace UserAPI.Services
{
    public class HelperService
    {
        public static string CurrentTime()
        {
            return DateTime.Now.ToString("G");
        }

        public static string CalcuteSHA256Hash(string rawData)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
                builder.Append(bytes[i].ToString("x2"));
            return builder.ToString();
        }

        public static string[] SplipFields(string fields, char separator = ',')
        {
            string result = "";
            foreach (char item in fields)
                if (item != ' ') result += item;
            return result.Split(separator);
        }
    }
}
