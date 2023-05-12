using System;
using System.IO;
using System.Data.SQLite;
using UserAPI.Configuration;
using Microsoft.Extensions.Configuration;

namespace UserAPI.Connector
{
  public class SQLiteConnector
  {
    private static SQLiteConnector connector;
    public SQLiteConfig Config { get; private set; }

    public SQLiteConnection connection {
      get; private set;
    }

    private SQLiteConnector(IConfigurationSection configuration)
    {
      Config = new SQLiteConfig();
      configuration.Bind(Config);
      string currentPath = Environment.CurrentDirectory;
      string projectDirectory = Directory.GetParent(currentPath).FullName;
      string connectString = string.Format("Data Source={0}; Version = 3;", projectDirectory + Config.Connect);
      connection = new SQLiteConnection();
      connection.ConnectionString = connectString;
    }

    public static SQLiteConnector GetInstance(IConfigurationSection configuration)
    {
      if (connector == null) connector = new SQLiteConnector(configuration);
      return connector;
    }

    public void OpenConnection()
    {
      connection.Open();
    }

    public void CloseConnection()
    {
      connection.Close();
    }
  }
}
