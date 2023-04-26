using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using UserAPI.Connector;
using UserAPI.Configuration;
using UserAPI.Models.CommonModel;
using UserAPI.Models.SQLiteModel;

namespace UserAPI.Services.SQLiteService
{
  public class UserSQLiteService : BaseService
  {
    public override void CreateTable(SQLiteConnector connecter)
    {
      string createCommand = string.Format("CREATE TABLE IF NOT EXISTS User {0}, {1}, {2}, {3}, {4})",
          "([id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT", "Username varchar(50)", "Password varchar(50)", "Age int", "Gender varchar(10)");
      SQLiteCommand command = new SQLiteCommand(createCommand, connecter.connection);
      command.ExecuteNonQuery();
    }

    public Result InsertUser(SQLiteUser user, SQLiteConnector connecter)
    {
      string insertCommand = string.Format("INSERT INTO User(Username,Password,Age,Gender) VALUES('{0}', '{1}', '{2}', '{3}')",
       user.Username, user.Password, user.Age, user.Gender);
      if (ExecuteCommand(insertCommand, connecter.connection))
      {
        string getIdCommand = "SELECT last_insert_rowid()";
        int _id = Convert.ToInt32(ExecuteScalar(getIdCommand, connecter.connection));
        return new Result
        {
          status = Status.Created,
          data = _id
        };
      }
      else return new Result
      {
        status = Status.BadRequest,
        data = Messages.BadRequest
      };
    }

    public Result Login(string username, string password, SQLiteConnector connecter)
    {
      DataSet data = new DataSet();
      SQLiteDataAdapter adapter = new SQLiteDataAdapter(
          string.Format("select id, Username, Password, Age, Gender from User where Username='{0}' AND Password='{1}'", username, password),
          connecter.connection);
      adapter.Fill(data);
      DataRow entity = data.Tables[0].Rows[0];
      SQLiteUser user = Utilities.ToObject<SQLiteUser>(entity);
      return new Result
      {
        status = Status.OK,
        data = user
      };
    }

    public SQLiteUser GetUserById(int userId, SQLiteConnector connecter)
    {
      DataSet data = new DataSet();
      SQLiteDataAdapter adapter = new SQLiteDataAdapter(
          string.Format("select id, Username, Password, Age, Gender from User where id='{0}'", userId),
          connecter.connection);
      adapter.Fill(data);
      DataRow entity = data.Tables[0].Rows[0];
      return Utilities.ToObject<SQLiteUser>(entity);
    }

    public IEnumerable<SQLiteUser> GetListUsers(SQLiteConnector connecter)
    {
      DataSet data = new DataSet();
      SQLiteDataAdapter adapter = new SQLiteDataAdapter(
          string.Format("select id, Username, Password, Age, Gender from User"),
          connecter.connection);
      adapter.Fill(data);
      DataRowCollection dataList = data.Tables[0].Rows;
      int count = dataList.Count;
      for (int i = 0; i < count; i++)
        yield return Utilities.ToObject<SQLiteUser>(dataList[i]);
    }

    public bool UpdateUserById(int userId, SQLiteUser update, SQLiteConnector connecter)
    {
      string updateCommand = string.Format("UPDATE User set Username='{0}', Password='{1}', Age='{2}', Gender='{3}' where id='{5}'",
       update.Username, update.Password, update.Age, update.Gender, userId);
      return ExecuteCommand(updateCommand, connecter.connection);
    }

    public bool DeleteUserById(int userId, SQLiteConnector connecter)
    {
      string updateCommand = string.Format("DELETE FROM User where id='{0}'", userId);
      return ExecuteCommand(updateCommand, connecter.connection);
    }
  }
}
