using UserAPI.Services.MongoService;
using UserAPI.Services.SQLiteService;
using UserAPI.Services.SqlService;
using UserAPI.Connector;

namespace UserAPI.Services
{
  public static class ServiceSelector
  {
    public static UserService userService { get; private set; }
    public static ProductService productService { get; private set; }
    public static UserSQLiteService userSQLiteService { get; private set; }
    public static EmployeeService employeeService { get; private set; }

    public static void InitUserService()
    {
      userService = new UserService("Users");
    }

    public static void InitProductionService()
    {
      productService = new ProductService("Products");
    }

    public static void InitUserSqliteService()
    {
      userSQLiteService = new UserSQLiteService();
      userSQLiteService.CreateTable(APIConnection.SQLite);
    }

    public static void InitEmployeeService()
    {
      employeeService = new EmployeeService();
    }
  }
}