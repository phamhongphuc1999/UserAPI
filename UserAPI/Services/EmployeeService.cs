using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using MongoDatabase;
using MongoDatabase.Entities;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserAPI.Models.SQLServer;

namespace UserAPI.Services
{
    public class EmployeeService: BaseService
    {
        public EmployeeService(IConfiguration configuration): base(configuration) { }

        public async Task<Result> InsertEmployee(InsertEmployeeInfo entity)
        {
            Employee employee = await SqlData.Employees.SingleOrDefaultAsync(x => x.Username == entity.Username);
            if(employee != null) return new Result
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
            EntityEntry result = SqlData.Employees.Add(newEmployee);
            SqlData.SaveChanges();
            if (result != null) return new Result
            {
                status = 200,
                data = result
            };
            return new Result
            {
                status = 400,
                data = "do not to create new user"
            };
        }

        public async Task<Result> GetEmployeeByUsername(string username, string[] fields = null)
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
            BsonDocument sEmployee = employee.ToBsonDocument();
            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach (string field in fields)
                if (Config.employeeFields.ContainsKey(field))
                    data.Add(field, sEmployee.GetElement(field).Value.ToString());
            return new Result
            {
                status = 200,
                data = data
            };
        }

        public async Task<Result> GetListEmployees(int pageSize = 0, int pageIndex = 0)
        {
            List<Employee> employeeList = await SqlData.Employees.ToListAsync();
            int totalResult = employeeList.Count;
            if (pageSize == 0) pageSize = totalResult;
            if (pageIndex == 0) pageIndex = 1;
            int index = pageSize * (pageIndex - 1);
            return new Result
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
        }

        public async Task<Result> UpdateEmployee(int employeeId, InsertEmployeeInfo updateEmployee)
        {
            Employee employee = await SqlData.Employees.FindAsync(employeeId);
            if (employee == null) return new Result
            {
                status = 400,
                data = $"the employee with id: {employeeId} do not exist"
            };
            if(updateEmployee.Username != null)
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
            SqlData.SaveChanges();
            Employee result = SqlData.Employees.Find(employeeId);
            return new Result
            {
                status = 200,
                data = result
            };
        }

        public async Task<Result> DeleteEmployee(int employeeId)
        {
            Employee employee = SqlData.Employees.Find(employeeId);
            if (employee == null) return new Result
            {
                status = 400,
                data = $"the employee with id: {employeeId} do not exist"
            };
            EntityEntry<Employee> result = SqlData.Remove(employee);
            return new Result
            {
                status = 200,
                data = result
            };
        }
    }
}
