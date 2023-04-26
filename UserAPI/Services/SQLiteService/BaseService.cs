using UserAPI.Connector;
using System.Data.SQLite;

namespace UserAPI.Services.SQLiteService
{
  public class BaseService
  {
    public virtual void CreateTable(SQLiteConnector connecter)
    {
    }

    protected virtual bool ExecuteCommand(string commandText, SQLiteConnection connecter)
    {
      SQLiteCommand command = new SQLiteCommand(commandText, connecter);
      return command.ExecuteNonQuery() > 0;
    }

    protected virtual object ExecuteScalar(string commandText, SQLiteConnection connecter)
    {
      SQLiteCommand command = new SQLiteCommand(commandText, connecter);
      return command.ExecuteScalar();
    }

    protected virtual SQLiteDataReader ExecuteReader(string commandText, SQLiteConnection connecter)
    {
      SQLiteCommand command = new SQLiteCommand(commandText, connecter);
      return command.ExecuteReader();
    }
  }
}
