using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using MongoDatabase.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
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
            EntityEntry result = sqlData.Employees.Add(newEmployee);
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

        public async Task<Result> GetListEmployees(int pageSize = Int32.MinValue, int pageIndex = Int32.MinValue)
        {
            List<Employee> employeeList = await SqlData.Employees.ToListAsync();
            int totalResult = employeeList.Count;
            if (pageSize == Int32.MinValue) pageSize = totalResult;
            if (pageIndex == Int32.MinValue) pageIndex = 1;
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
    }
}
