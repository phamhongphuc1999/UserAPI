using MySqlConnector;
using System.Collections.Generic;

namespace UserAPI.Models.MySqlModel.DataSet
{
  public class SqlDataSet<T> : BaseSqlDataSet<T> where T : new()
  {
    public SqlDataSet(MySqlConnection connection) : base(connection) { }

    public List<T> Execute(MySqlCommand executedCommand)
    {
      MySqlDataReader reader = executedCommand.ExecuteReader();
      List<T> result = new List<T>();
      try
      {
        while (reader.Read())
        {
          T _item = this.ConvertDataSetToObject(reader);
          result.Add(_item);
        }
      }
      finally
      {
        reader.Close();
      }
      return result;
    }

    public List<T> SelectAll(string projectCommand)
    {
      MySqlCommand command = new MySqlCommand($"SELECT %s{projectCommand} FROM %s{this.tableName};", this.connection);
      return this.Execute(command);
    }

    public List<T> SelectWithFilter(string filterCommand, string projectCommand)
    {
      MySqlCommand command = new MySqlCommand($"SELECT %s{projectCommand} FROM %s{this.tableName} %s{filterCommand};", this.connection);
      return this.Execute(command);
    }

    public List<T> InsertSingle(string insertedElement, string insertedValue)
    {
      MySqlCommand command = new MySqlCommand($"INSERT INTO %s{this.tableName} (%s{insertedElement}) VALUES (%s{insertedValue});");
      return this.Execute(command);
    }
  }
}