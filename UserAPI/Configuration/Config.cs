namespace UserAPI.Configuration
{
  public class JWTConfig
  {
    public int ExpireMinutes { get; set; }
    public string SecretKey { get; set; }
  }

  public class MongoConfig
  {
    public string Connect { get; set; }
    public string Database { get; set; }
  }

  public class SQLConfig
  {
    public string ConnectString { get; set; }
  }

  public class SQLiteConfig
  {
    public string Connect { get; set; }
  }

  public class DevelopmentConfig
  {
    public string ApplicationUrl { get; set; }
    public string Version { get; set; }
    public string ApplicationName { get; set; }
  }

  public class ProductionConfig
  {
    public string ApplicationUrl { get; set; }
    public string Version { get; set; }
    public string ApplicationName { get; set; }
  }
}
