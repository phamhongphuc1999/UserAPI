using System.Data.SQLite;
using UserAPI.Connector;

namespace UserAPI.Services.SQLiteService
{
  public class TemplateService : BaseService
  {
    public override void CreateTable(SQLiteConnector connecter)
    {
      string createCommand = string.Format("CREATE TABLE IF NOT EXISTS Template {0}, {1}, {2}, {3})",
          "([id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT", "Name varchar(50)", "Type varchar(50)", "Description varchar(100)");
      SQLiteCommand command = new SQLiteCommand(createCommand, connecter.connection);
      command.ExecuteNonQuery();
    }
  }
}
