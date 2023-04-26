using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserAPI.Configuration;
using UserAPI.Models.SQLServerModel;

namespace UserAPI.Connector
{
  public class SQLConnector
  {
    public SQLData SqlData { get; private set; }
    public DbContextOptionsBuilder<SQLData> Option { get; private set; }
    public SQLConfig Config { get; private set; }
    private static SQLConnector connecter;

    private SQLConnector(IConfigurationSection configuration)
    {
      Config = new SQLConfig();
      configuration.Bind(Config);
      Option = new DbContextOptionsBuilder<SQLData>();
      Option.UseSqlServer(Config.Connect);
      SqlData = new SQLData(Option.Options);
    }

    public static SQLConnector GetInstance(IConfigurationSection configuration)
    {
      if (connecter == null) connecter = new SQLConnector(configuration);
      return connecter;
    }
  }
}
