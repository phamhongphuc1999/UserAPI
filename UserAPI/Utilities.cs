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

using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace UserAPI
{
    public static class Utilities
    {
        /// <summary>
        /// Convert raw string with SHA256
        /// </summary>
        /// <param name="rawData">The raw string</param>
        /// <returns>The string repersenting SHA365 hash result</returns>
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
        /// remove space and splip string to array
        /// </summary>
        /// <param name="fields">The input string</param>
        /// <param name="separator">The char to splip</param>
        /// <returns>The result string array</returns>
        public static string[] SplipFields(string fields, char separator = ',')
        {
            string result = "";
            foreach (char item in fields)
                if (item != ' ') result += item;
            return result.Split(separator);
        }

        /// <summary>
        /// Convert string to DateTime
        /// </summary>
        /// <param name="time">The string representing DateTime</param>
        /// <param name="format">The string date format</param>
        /// <returns>The DateTime object representing the input string</returns>
        public static DateTime ConvertStringToTime(string time, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return DateTime.ParseExact(time, format, System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Check token
        /// </summary>
        /// <param name="token">The input token</param>
        /// <returns>The token is valid return true or return false if token is invalid</returns>
        public static bool IsValidToken(string token)
        {
            return token != "" && token != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static PropertyInfo GetProperty(Type type, string attributeName)
        {
            PropertyInfo property = type.GetProperty(attributeName);
            if (property != null) return property;
            return type.GetProperties()
                 .Where(p => p.IsDefined(typeof(DisplayAttribute), false) && p.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().Single().Name == attributeName)
                 .FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ChangeType(object value, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null) return null;
                return Convert.ChangeType(value, Nullable.GetUnderlyingType(type));
            }
            return Convert.ChangeType(value, type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public static T ToObject<T>(this DataRow dataRow) where T : new()
        {
            T item = new T();
            foreach (DataColumn column in dataRow.Table.Columns)
            {
                PropertyInfo property = GetProperty(typeof(T), column.ColumnName);
                if (property != null && dataRow[column] != DBNull.Value && dataRow[column].ToString() != "NULL")
                    property.SetValue(item, ChangeType(dataRow[column], property.PropertyType), null);
            }
            return item;
        }
    }
}
