using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using UserAPI.Configuration;

namespace UserAPI.Connector
{
  public class MongoConnector
  {
    public MongoClient Client { get; private set; }
    public IMongoDatabase MDatabase { get; private set; }
    public MongoConfig Config { get; private set; }

    private static MongoConnector connector;

    protected MongoConnector(IConfigurationSection configuration)
    {
      Config = new MongoConfig();
      configuration.Bind(Config);
      Client = new MongoClient(Config.Connect);
      MDatabase = Client.GetDatabase(Config.Database);
    }

    public static MongoConnector GetInstance(IConfigurationSection configuration)
    {
      if (connector == null) connector = new MongoConnector(configuration);
      return connector;
    }
  }
}
