using UserAPI.Models.MySqlModel;
using UserAPI.Connector;

namespace UserAPI.Services.SqlService
{
  public class EmployeeService : BaseService<Employee>
  {
    public EmployeeService() : base(APIConnection.SQL.sqlData.employee) { }
  }
}