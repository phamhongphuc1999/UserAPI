using UserAPI.Models.MySqlModel;
using UserAPI.Connector;
using System.Collections.Generic;
using UserAPI.Models.CommonModel;
using UserAPI.Configuration;

namespace UserAPI.Services.SqlService
{
  public class EmployeeService : BaseService<Employee>
  {
    public EmployeeService() : base(APIConnection.SQL.sqlData.employee) { }

    public Result Login(string username, string password)
    {
      List<Employee> result = this.SelectWithFilter($"WHERE username=%s{username} AND password=%s{password}", null);
      if (result.Count > 0) return new Result { status = Status.OK, data = result[0] };
      else return new Result { status = Status.BadRequest, data = null };
    }

    public Result Register(string username, string password, string email)
    {
      List<Employee> check = this.SelectWithFilter($"WHERE username=%s{username}", null);
      if (check.Count > 0) return new Result { status = Status.BadRequest, data = $"{username} has been used" };
      List<Employee> result = this.InsertSingle("username, password, email", $"%s{username}, %s{password}, %s{email}");
      if (result.Count > 0) return new Result { status = Status.OK, data = result[0] };
      else return new Result { status = Status.BadRequest, data = null };
    }

    public Result GetEmployeeById(string employeeId, string fields = null)
    {
      List<Employee> result = this.SelectWithFilter($"WHERE id=${employeeId}", fields);
      if (result.Count > 0) return new Result { status = Status.OK, data = result[0] };
      else return new Result { status = Status.BadRequest, data = null };
    }

    public Result GetEmployeeByUsername(string username, string fields = null)
    {
      List<Employee> result = this.SelectWithFilter($"WHERE username=${username}", fields);
      if (result.Count > 0) return new Result { status = Status.OK, data = result[0] };
      else return new Result { status = Status.BadRequest, data = null };
    }
  }
}
