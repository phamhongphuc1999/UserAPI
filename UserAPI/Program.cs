using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserAPI.Connector;
using UserAPI.Services;

namespace UserAPI
{
  public class Program
  {
    public static void Main(string[] args)
    {
      IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
      IConfigurationSection mongoSetting = config.GetSection("MongoSetting");
      IConfigurationSection sqlSetting = config.GetSection("SQLSetting");
      IConfigurationSection sqliteSetting = config.GetSection("SQLiteSetting");

      APIConnection.InitSqlConnection(sqlSetting);
      APIConnection.InitMongoConnection(mongoSetting);
      APIConnection.InitSqliteConnection(sqliteSetting);

      ServiceSelector.Mongo.Init();
      ServiceSelector.Lite.Init();
      ServiceSelector.Sql.Init();

      IHostBuilder builder = CreateHostBuilder(args);
      builder.Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
      return Host.CreateDefaultBuilder(args)
          .ConfigureLogging(logging =>
          {
            logging.ClearProviders();
            logging.AddConsole();
          })
          .ConfigureWebHostDefaults(webBuilder =>
          {
            webBuilder.UseStartup<Startup>();
          });
    }
  }
}
