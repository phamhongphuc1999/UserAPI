// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using System;
using System.Security.Cryptography;
using System.Text;

namespace UserAPI.Services
{
    public class HelperService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static string CalcuteSHA256Hash(string rawData)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
                builder.Append(bytes[i].ToString("x2"));
            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] SplipFields(string fields, char separator = ',')
        {
            string result = "";
            foreach (char item in fields)
                if (item != ' ') result += item;
            return result.Split(separator);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime ConvertStringToTime(string time)
        {
            return DateTime.ParseExact(time , "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
