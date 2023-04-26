using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using UserAPI.Configuration;

namespace UserAPI.Models.MySqlModel.DataSet
{
  public class BaseSqlDataSet<T> where T : new()
  {
    public MySqlConnection connection;
    public string tableName;
    public List<TableInfo> tableInfo;

    public BaseSqlDataSet(MySqlConnection connection)
    {
      this.connection = connection;
      FullData moduleInfo = this.GetReflection();
      List<string> tableSchema = this.GetTableSchema(moduleInfo.ModuleName);
      List<TableInfo> result = new List<TableInfo>();
      foreach (TableInfo item in moduleInfo.ModuleInfo)
      {
        string name = item.AttributeName == null ? item.Name : item.AttributeName;
        bool check = tableSchema.Exists(x => x == name);
        if (!check) throw new Exception($"{name} element is not exist");
      }
      this.tableName = moduleInfo.ModuleName;
      this.tableInfo = result;
    }

    protected FullData GetReflection()
    {
      Type moduleInfo = typeof(T);
      System.Reflection.PropertyInfo[] properties = moduleInfo.GetProperties();
      List<TableInfo> result = new List<TableInfo>();
      foreach (System.Reflection.PropertyInfo _pro in properties)
      {
        Attribute[] _proAttribute = Attribute.GetCustomAttributes(_pro);
        int count = 0;
        bool check = true;
        int len = _proAttribute.Length;
        while (count < len && check)
        {
          if (_proAttribute[count].GetType() == typeof(TableRow))
          {
            TableRow _proTableRow = _proAttribute[count] as TableRow;
            result.Add(new TableInfo { Name = _pro.Name, PropertyType = _pro.PropertyType.Name, AttributeName = _proTableRow.Name });
            check = false;
          }
          count++;
        }
        if (check) result.Add(new TableInfo { Name = _pro.Name, PropertyType = _pro.PropertyType.Name, AttributeName = null });
      }
      Attribute[] attrs = Attribute.GetCustomAttributes(moduleInfo);
      string moduleName = moduleInfo.Name;
      int _count = 0;
      bool _check = true;
      int _len = attrs.Length;
      while (_count < _len && _check)
      {
        if (attrs[_count].GetType() == typeof(Table))
        {
          Table a = attrs[_count] as Table;
          moduleName = a.Name;
        }
        _count++;
      }
      return new FullData { ModuleInfo = result, ModuleName = moduleName };
    }

    protected List<string> GetTableSchema(string tableName)
    {
      MySqlCommand command = new MySqlCommand("select COLUMN_NAME, DATA_TYPE as 'name' from information_schema.columns where table_name=@t_name;", this.connection);
      command.Parameters.AddWithValue("@t_name", tableName);
      DataTable data = new DataTable();
      MySqlDataAdapter da = new MySqlDataAdapter(command);
      da.Fill(data);
      List<string> result = new List<string>();
      foreach (DataRow _row in data.Rows)
      {
        result.Add(_row[0].ToString());
      }
      if (result.Count == 0) throw new Exception("Row is empty");
      return result;
    }

    protected void SetObjectValue<A>(MySqlDataReader data, string tableName, T obj, string propertyName)
    {
      try
      {
        A _value = data.GetFieldValue<A>(tableName);
        obj.GetType().GetProperty(propertyName).SetValue(obj, _value);
      }
      catch
      {
        obj.GetType().GetProperty(propertyName).SetValue(obj, null);
      }
    }

    public T ConvertDataSetToObject(MySqlDataReader data)
    {
      T result = new T();
      foreach (TableInfo item in this.tableInfo)
      {
        string name = item.Name;
        string tableName = item.AttributeName;
        string _type = item.PropertyType;
        if (_type == "String") this.SetObjectValue<string>(data, tableName, result, name);
        else if (_type == "Int16") this.SetObjectValue<Int16>(data, tableName, result, name);
        else if (_type == "Int32") this.SetObjectValue<Int32>(data, tableName, result, name);
        else this.SetObjectValue<string>(data, tableName, result, name);
      }
      return result;
    }
  }
}
