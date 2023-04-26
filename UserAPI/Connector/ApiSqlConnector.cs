using UserAPI.Configuration;
using MySqlConnector;
using UserAPI.Models.MySqlModel;
using Microsoft.Extensions.Configuration;

namespace UserAPI.Connector
{
  public class ApiSqlConnector
  {
    private static ApiSqlConnector connector;
    public SQLConfig config;
    public MySqlConnection connection;
    public SqlData sqlData;

    protected ApiSqlConnector(IConfigurationSection configuration)
    {
      this.config = new SQLConfig();
      configuration.Bind(this.config);
      this.connection = new MySqlConnection(this.config.ConnectString);
      this.connection.Open();
      this.sqlData = new SqlData(this.connection);
    }

    public static ApiSqlConnector GetInstance(IConfigurationSection configuration)
    {
      if (ApiSqlConnector.connector == null) ApiSqlConnector.connector = new ApiSqlConnector(configuration);
      return ApiSqlConnector.connector;
    }
  }
}