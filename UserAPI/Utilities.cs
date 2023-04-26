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
    public static string CalculateSHA256Hash(string rawData)
    {
      SHA256 sha256 = SHA256.Create();
      byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
      StringBuilder builder = new StringBuilder();
      for (int i = 0; i < bytes.Length; i++)
        builder.Append(bytes[i].ToString("x2"));
      return builder.ToString();
    }

    public static string[] SplitFields(string fields, char separator = ',')
    {
      string result = "";
      foreach (char item in fields)
        if (item != ' ') result += item;
      return result.Split(separator);
    }

    public static DateTime ConvertStringToTime(string time, string format = "yyyy-MM-dd HH:mm:ss")
    {
      return DateTime.ParseExact(time, format, System.Globalization.CultureInfo.InvariantCulture);
    }

    public static bool IsValidToken(string token)
    {
      return token != "" && token != null;
    }

    public static PropertyInfo GetProperty(Type type, string attributeName)
    {
      PropertyInfo property = type.GetProperty(attributeName);
      if (property != null) return property;
      return type.GetProperties()
           .Where(p => p.IsDefined(typeof(DisplayAttribute), false) && p.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().Single().Name == attributeName)
           .FirstOrDefault();
    }

    public static object ChangeType(object value, Type type)
    {
      if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
      {
        if (value == null) return null;
        return Convert.ChangeType(value, Nullable.GetUnderlyingType(type));
      }
      return Convert.ChangeType(value, type);
    }

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
