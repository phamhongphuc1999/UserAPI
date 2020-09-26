using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDatabase;
using MongoDatabase.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.Models.SQLServer;

namespace UserAPI.Services.DatabaseService
{
    public class EmployeeService : BaseService
    {
        public EmployeeService(IConfiguration configuration) : base(configuration) { }

        public Result InsertEmployee(InsertEmployeeInfo entity)
        {
            Employee employee = SqlData.Employees.SingleOrDefault(x => x.Username == entity.Username);
            if (employee != null) return new Result
            {
                status = 400,
                data = $"username {entity.Username} have existed"
            };
            Employee newEmployee = new Employee()
            {
                Name = entity.Name,
                Username = entity.Username,
                Password = entity.Password,
                Sex = entity.Sex,
                Phone = entity.Phone,
                Address = entity.Address,
                Position = entity.Position,
                Image = entity.Image ?? null,
                Birthday = entity.Birthday ?? null,
                Node = entity.Node ?? null
            };
            SqlData.Employees.Add(newEmployee);
            int check = SqlData.SaveChanges();
            if (check > 0) return new Result
            {
                status = 200,
                data = newEmployee
            };
            return new Result
            {
                status = 400,
                data = "do not to create new user"
            };
        }

        public async Task<Result> InsertEmployeeAsync(InsertEmployeeInfo entity)
        {
            Employee employee = await SqlData.Employees.SingleOrDefaultAsync(x => x.Username == entity.Username);
            if (employee != null) return new Result
            {
                status = 400,
                data = $"username {entity.Username} have existed"
            };
            Employee newEmployee = new Employee()
            {
                Name = entity.Name,
                Username = entity.Username,
                Password = entity.Password,
                Sex = entity.Sex,
                Phone = entity.Phone,
                Address = entity.Address,
                Position = entity.Position,
                Image = entity.Image ?? null,
                Birthday = entity.Birthday ?? null,
                Node = entity.Node ?? null
            };
            await SqlData.Employees.AddAsync(newEmployee);
            int check = await SqlData.SaveChangesAsync();
            if (check > 0) return new Result
            {
                status = 200,
                data = newEmployee
            };
            return new Result
            {
                status = 400,
                data = "do not to create new user"
            };
        }

        public Result GetEmployeeByUsername(string username, string[] fields = null)
        {
            Employee employee = SqlData.Employees.SingleOrDefault(x => x.Username == username);
            if (employee == null) return new Result
            {
                status = 400,
                data = $"the employee with username: {username} do not exist"
            };
            if (fields == null) return new Result
            {
                status = 200,
                data = employee
            };
            List<(string, object)> data = new List<(string, object)>();
            foreach (string field in fields)
                if (Config.employeeFields.ContainsKey(field))
                    data.Add((field, employee.GetType().GetProperty(field).GetValue(employee)));
            return new Result
            {
                status = 200,
                data = data
            };
        }

        public async Task<Result> GetEmployeeByUsernameAsync(string username, string[] fields = null)
        {
            Employee employee = await SqlData.Employees.SingleOrDefaultAsync(x => x.Username == username);
            if (employee == null) return new Result
            {
                status = 400,
                data = $"the employee with username: {username} do not exist"
            };
            if (fields == null) return new Result
            {
                status = 200,
                data = employee
            };
            List<(string, object)> data = new List<(string, object)>();
            foreach (string field in fields)
                if (Config.employeeFields.ContainsKey(field))
                    data.Add((field, employee.GetType().GetProperty(field).GetValue(employee)));
            return new Result
            {
                status = 200,
                data = data
            };
        }

        public Result GetListEmployees(int pageSize = 0, int pageIndex = 0, string[] fields = null)
        {
            List<Employee> employeeList = SqlData.Employees.ToList();
            int totalResult = employeeList.Count;
            if (pageSize == 0) pageSize = totalResult;
            if (pageIndex == 0) pageIndex = 1;
            int index = pageSize * (pageIndex - 1);
            if (fields == null) return new Result
            {
                status = 200,
                data = new
                {
                    user_list = employeeList.GetRange(index, pageSize),
                    pagination = new
                    {
                        totalResult = totalResult,
                        pageIndex = pageIndex,
                        pageSize = pageSize
                    }
                }
            };
            List<Employee> tempList = employeeList.GetRange(index, pageSize);
            IEnumerable<List<(string, object)>> employeeFilterList = tempList.Select(e =>
            {
                List<(string, object)> result = new List<(string, object)>();
                foreach (string field in fields)
                {
                    object value = e.GetType().GetProperty(field).GetValue(e);
                    result.Add((field, value));
                }
                return result;
            });
            return new Result
            {
                status = 200,
                data = new
                {
                    user_list = employeeFilterList,
                    pagination = new
                    {
                        totalResult = totalResult,
                        pageIndex = pageIndex,
                        pageSize = pageSize
                    }
                }
            };
        }

        public async Task<Result> GetListEmployeesAsync(int pageSize = 0, int pageIndex = 0, string[] fields = null)
        {
            List<Employee> employeeList = await SqlData.Employees.ToListAsync();
            int totalResult = employeeList.Count;
            if (pageSize == 0) pageSize = totalResult;
            if (pageIndex == 0) pageIndex = 1;
            int index = pageSize * (pageIndex - 1);
            if (fields == null) return new Result
            {
                status = 200,
                data = new
                {
                    user_list = employeeList.GetRange(index, pageSize),
                    pagination = new
                    {
                        totalResult = totalResult,
                        pageIndex = pageIndex,
                        pageSize = pageSize
                    }
                }
            };
            List<Employee> tempList = employeeList.GetRange(index, pageSize);
            IEnumerable<List<(string, object)>> employeeFilterList = tempList.Select(e =>
            {
                List<(string, object)> result = new List<(string, object)>();
                foreach (string field in fields)
                {
                    object value = e.GetType().GetProperty(field).GetValue(e);
                    result.Add((field, value));
                }
                return result;
            });
            return new Result
            {
                status = 200,
                data = new
                {
                    user_list = employeeFilterList,
                    pagination = new
                    {
                        totalResult = totalResult,
                        pageIndex = pageIndex,
                        pageSize = pageSize
                    }
                }
            };
        }

        public Result UpdateEmployee(int employeeId, InsertEmployeeInfo updateEmployee)
        {
            Employee employee = SqlData.Employees.Find(employeeId);
            if (employee == null) return new Result
            {
                status = 400,
                data = $"the employee with id: {employeeId} do not exist"
            };
            if (updateEmployee.Username != null)
            {
                Employee checkEmployee = SqlData.Employees.SingleOrDefault(x => x.Username == updateEmployee.Username);
                if (checkEmployee != null) return new Result
                {
                    status = 400,
                    data = $"the username: {updateEmployee.Username} is exist"
                };
                employee.Username = updateEmployee.Username;
            }
            if (updateEmployee.Password != null)
            {
                string newPassword = SHA256Hash.CalcuteHash(updateEmployee.Password);
                employee.Password = newPassword;
            }
            if (updateEmployee.Name != null) employee.Name = updateEmployee.Name;
            if (updateEmployee.Image != null) employee.Image = updateEmployee.Image;
            if (updateEmployee.Phone != null) employee.Phone = updateEmployee.Phone;
            if (updateEmployee.Position != null) employee.Position = updateEmployee.Position;
            if (updateEmployee.Sex != null) employee.Sex = updateEmployee.Sex;
            if (updateEmployee.Birthday != null) employee.Birthday = updateEmployee.Birthday;
            if (updateEmployee.Node != null) employee.Node = updateEmployee.Node;
            int check = SqlData.SaveChanges();
            Employee result = SqlData.Employees.Find(employeeId);
            if (check > 0) return new Result
            {
                status = 200,
                data = result
            };
            else return new Result
            {
                status = 400,
                data = $"do not update employee with id: {employeeId}"
            };
        }

        public async Task<Result> UpdateEmployeeAsync(int employeeId, InsertEmployeeInfo updateEmployee)
        {
            Employee employee = await SqlData.Employees.FindAsync(employeeId);
            if (employee == null) return new Result
            {
                status = 400,
                data = $"the employee with id: {employeeId} do not exist"
            };
            if (updateEmployee.Username != null)
            {
                Employee checkEmployee = await SqlData.Employees.SingleOrDefaultAsync(x => x.Username == updateEmployee.Username);
                if (checkEmployee != null) return new Result
                {
                    status = 400,
                    data = $"the username: {updateEmployee.Username} is exist"
                };
                employee.Username = updateEmployee.Username;
            }
            if (updateEmployee.Password != null)
            {
                string newPassword = SHA256Hash.CalcuteHash(updateEmployee.Password);
                employee.Password = newPassword;
            }
            if (updateEmployee.Name != null) employee.Name = updateEmployee.Name;
            if (updateEmployee.Image != null) employee.Image = updateEmployee.Image;
            if (updateEmployee.Phone != null) employee.Phone = updateEmployee.Phone;
            if (updateEmployee.Position != null) employee.Position = updateEmployee.Position;
            if (updateEmployee.Sex != null) employee.Sex = updateEmployee.Sex;
            if (updateEmployee.Birthday != null) employee.Birthday = updateEmployee.Birthday;
            if (updateEmployee.Node != null) employee.Node = updateEmployee.Node;
            int check = await SqlData.SaveChangesAsync();
            Employee result = await SqlData.Employees.FindAsync(employeeId);
            if (check > 0) return new Result
            {
                status = 200,
                data = result
            };
            else return new Result
            {
                status = 400,
                data = $"do not update employee with id: {employeeId}"
            };
        }

        public Result DeleteEmployee(int employeeId)
        {
            Employee employee = SqlData.Employees.Find(employeeId);
            if (employee == null) return new Result
            {
                status = 400,
                data = $"the employee with id: {employeeId} do not exist"
            };
            SqlData.Remove(employee);
            int check = SqlData.SaveChanges();
            if (check > 0) return new Result
            {
                status = 200,
                data = employee
            };
            else return new Result
            {
                status = 400,
                data = $"do not delete employee with id: {employeeId}"
            };
        }

        public async Task<Result> DeleteEmployeeAsync(int employeeId)
        {
            Employee employee = await SqlData.Employees.FindAsync(employeeId);
            if (employee == null) return new Result
            {
                status = 400,
                data = $"the employee with id: {employeeId} do not exist"
            };
            SqlData.Remove(employee);
            int check = await SqlData.SaveChangesAsync();
            if (check > 0) return new Result
            {
                status = 200,
                data = employee
            };
            else return new Result
            {
                status = 400,
                data = $"do not delete employee with id: {employeeId}"
            };
        }
    }
}
