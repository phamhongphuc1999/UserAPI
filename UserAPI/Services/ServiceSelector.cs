using UserAPI.Services.MongoService;
using UserAPI.Services.SQLiteService;
using UserAPI.Services.SqlService;
using UserAPI.Connector;

namespace UserAPI.Services
{
  public static class ServiceSelector
  {
    public static class Mongo
    {
      public static UserService user { get; private set; }
      public static ProductService product { get; private set; }

      public static void Init()
      {
        user = new UserService("Users");
        product = new ProductService("Products");
      }
    }

    public static class Lite
    {
      public static UserSQLiteService user { get; private set; }
      public static TemplateService template { get; private set; }

      public static void Init()
      {
        user = new UserSQLiteService();
        template = new TemplateService();
        user.CreateTable(APIConnection.SQLite);
      }
    }

    public static class Sql
    {
      public static EmployeeService employee { get; private set; }
      public static ProductionService product { get; private set; }

      public static void Init()
      {
        employee = new EmployeeService();
        product = new ProductionService();
      }
    }
  }
}
