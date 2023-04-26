using Microsoft.Extensions.Configuration;

namespace UserAPI.Connector
{
  public static class APIConnection
  {
    public static ApiSqlConnector SQL { get; private set; }
    public static MongoConnector Mongo { get; private set; }
    public static SQLiteConnector SQLite { get; private set; }

    public static void InitSqlConnection(IConfigurationSection config)
    {
      if (SQL == null) SQL = ApiSqlConnector.GetInstance(config);
    }

    public static void InitMongoConnection(IConfigurationSection config)
    {
      if (Mongo == null) Mongo = MongoConnector.GetInstance(config);
    }

    public static void InitSqliteConnection(IConfigurationSection config)
    {
      if (SQLite == null) SQLite = SQLiteConnector.GetInstance(config);
      SQLite.OpenConnection();
    }
  }
}
