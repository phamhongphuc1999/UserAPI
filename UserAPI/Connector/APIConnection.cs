using Microsoft.Extensions.Configuration;

namespace UserAPI.Connector
{
  public class APIConnection
  {
    public MongoConnector Mongo { get; private set; }
    public SQLConnector SQL { get; private set; }
    public SQLiteConnector SQLite { get; private set; }

    public APIConnection(IConfigurationSection mongoConfig, IConfigurationSection sqlConfig, IConfigurationSection sqliteConfig)
    {
      Mongo = MongoConnector.GetInstance(mongoConfig);
      SQL = SQLConnector.GetInstance(sqlConfig);
      SQLite = SQLiteConnector.GetInstance(sqliteConfig);
      SQLite.OpenConnection();
    }
  }
}
