// -------------------- SIMPLE API -------------------- 
//
//
// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
//
//
// Product by: Pham Hong Phuc
//
//
// ----------------------------------------------------

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.Contances;
using UserAPI.Models.CommonModel;
using UserAPI.Models.SQLServerModel;
using static UserAPI.Program;

namespace UserAPI.Services.SQLServerService
{
    public class EmployeeService
    {
        public Result InsertEmployee(InsertEmployeeInfo entity)
        {
            Employee employee = APIConnecter.SQL.SqlData.Employees.SingleOrDefault(x => x.Username == entity.Username);
            if (employee != null) return new Result
            {
                status = Status.BadRequest,
                data = Messages.ExistedUser
            };
            Employee newEmployee = new Employee()
            {
                Name = entity.Name,
                Username = entity.Username,
                Password = entity.Password,
                Gender = entity.Gender,
                Phone = entity.Phone,
                Address = entity.Address,
                Position = entity.Position,
                Image = entity.Image ?? null,
                Birthday = entity.Birthday ?? null,
                Node = entity.Node ?? null
            };
            APIConnecter.SQL.SqlData.Employees.Add(newEmployee);
            int check = APIConnecter.SQL.SqlData.SaveChanges();
            if (check > 0) return new Result
            {
                status = Status.OK,
                data = newEmployee
            };
            return new Result
            {
                status = Status.BadRequest,
                data = Messages.BadRequest
            };
        }

        public async Task<Result> InsertEmployeeAsync(InsertEmployeeInfo entity)
        {
            Employee employee = await APIConnecter.SQL.SqlData.Employees.SingleOrDefaultAsync(x => x.Username == entity.Username);
            if (employee != null) return new Result
            {
                status = Status.BadRequest,
                data = Messages.ExistedUser
            };
            Employee newEmployee = new Employee()
            {
                Name = entity.Name,
                Username = entity.Username,
                Password = entity.Password,
                Gender = entity.Gender,
                Phone = entity.Phone,
                Address = entity.Address,
                Position = entity.Position,
                Image = entity.Image ?? null,
                Birthday = entity.Birthday ?? null,
                Node = entity.Node ?? null
            };
            await APIConnecter.SQL.SqlData.Employees.AddAsync(newEmployee);
            int check = await APIConnecter.SQL.SqlData.SaveChangesAsync();
            if (check > 0) return new Result
            {
                status = Status.OK,
                data = newEmployee
            };
            return new Result
            {
                status = Status.BadRequest,
                data = Messages.BadRequest
            };
        }

        public Result GetEmployeeByUsername(string username, string[] fields = null)
        {
            Employee employee = APIConnecter.SQL.SqlData.Employees.SingleOrDefault(x => x.Username == username);
            if (employee == null) return new Result
            {
                status = Status.BadRequest,
                data = Messages.BadRequest
            };
            if (fields == null) return new Result
            {
                status = Status.OK,
                data = employee
            };
            List<(string, object)> data = new List<(string, object)>();
            foreach (string field in fields)
                if (APIConnecter.SQL.Config.EmployeeFields.Contains(field))
                    data.Add((field, employee.GetType().GetProperty(field).GetValue(employee)));
            return new Result
            {
                status = Status.OK,
                data = data
            };
        }

        public async Task<Result> GetEmployeeByUsernameAsync(string username, string[] fields = null)
        {
            Employee employee = await APIConnecter.SQL.SqlData.Employees.SingleOrDefaultAsync(x => x.Username == username);
            if (employee == null) return new Result
            {
                status = Status.BadRequest,
                data = Messages.BadRequest
            };
            if (fields == null) return new Result
            {
                status = Status.OK,
                data = employee
            };
            List<(string, object)> data = new List<(string, object)>();
            foreach (string field in fields)
                if (APIConnecter.SQL.Config.EmployeeFields.Contains(field))
                    data.Add((field, employee.GetType().GetProperty(field).GetValue(employee)));
            return new Result
            {
                status = Status.OK,
                data = data
            };
        }

        public Result GetListEmployees(int pageSize = 0, int pageIndex = 0, string[] fields = null)
        {
            List<Employee> employeeList = APIConnecter.SQL.SqlData.Employees.ToList();
            int totalResult = employeeList.Count;
            if (pageSize == 0) pageSize = totalResult;
            if (pageIndex == 0) pageIndex = 1;
            int index = pageSize * (pageIndex - 1);
            if (fields == null) return new Result
            {
                status = Status.OK,
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
                status = Status.OK,
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
            List<Employee> employeeList = await APIConnecter.SQL.SqlData.Employees.ToListAsync();
            int totalResult = employeeList.Count;
            if (pageSize == 0) pageSize = totalResult;
            if (pageIndex == 0) pageIndex = 1;
            int index = pageSize * (pageIndex - 1);
            if (fields == null) return new Result
            {
                status = Status.OK,
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
                status = Status.OK,
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
            Employee employee = APIConnecter.SQL.SqlData.Employees.Find(employeeId);
            if (employee == null) return new Result
            {
                status = Status.BadRequest,
                data = Messages.BadRequest
            };
            if (updateEmployee.Username != null)
            {
                Employee checkEmployee = APIConnecter.SQL.SqlData.Employees.SingleOrDefault(x => x.Username == updateEmployee.Username);
                if (checkEmployee != null) return new Result
                {
                    status = Status.BadRequest,
                    data = Messages.BadRequest
                };
                employee.Username = updateEmployee.Username;
            }
            if (updateEmployee.Password != null)
            {
                string newPassword = Utilities.CalcuteSHA256Hash(updateEmployee.Password);
                employee.Password = newPassword;
            }
            if (updateEmployee.Name != null) employee.Name = updateEmployee.Name;
            if (updateEmployee.Image != null) employee.Image = updateEmployee.Image;
            if (updateEmployee.Phone != null) employee.Phone = updateEmployee.Phone;
            if (updateEmployee.Position != null) employee.Position = updateEmployee.Position;
            if (updateEmployee.Gender != null) employee.Gender = updateEmployee.Gender;
            if (updateEmployee.Birthday != null) employee.Birthday = updateEmployee.Birthday;
            if (updateEmployee.Node != null) employee.Node = updateEmployee.Node;
            int check = APIConnecter.SQL.SqlData.SaveChanges();
            Employee result = APIConnecter.SQL.SqlData.Employees.Find(employeeId);
            if (check > 0) return new Result
            {
                status = Status.OK,
                data = result
            };
            else return new Result
            {
                status = Status.BadRequest,
                data = Messages.BadRequest
            };
        }

        public async Task<Result> UpdateEmployeeAsync(int employeeId, InsertEmployeeInfo updateEmployee)
        {
            Employee employee = await APIConnecter.SQL.SqlData.Employees.FindAsync(employeeId);
            if (employee == null) return new Result
            {
                status = Status.BadRequest,
                data = Messages.BadRequest
            };
            if (updateEmployee.Username != null)
            {
                Employee checkEmployee = await APIConnecter.SQL.SqlData.Employees.SingleOrDefaultAsync(x => x.Username == updateEmployee.Username);
                if (checkEmployee != null) return new Result
                {
                    status = Status.BadRequest,
                    data = Messages.BadRequest
                };
                employee.Username = updateEmployee.Username;
            }
            if (updateEmployee.Password != null)
            {
                string newPassword = Utilities.CalcuteSHA256Hash(updateEmployee.Password);
                employee.Password = newPassword;
            }
            if (updateEmployee.Name != null) employee.Name = updateEmployee.Name;
            if (updateEmployee.Image != null) employee.Image = updateEmployee.Image;
            if (updateEmployee.Phone != null) employee.Phone = updateEmployee.Phone;
            if (updateEmployee.Position != null) employee.Position = updateEmployee.Position;
            if (updateEmployee.Gender != null) employee.Gender = updateEmployee.Gender;
            if (updateEmployee.Birthday != null) employee.Birthday = updateEmployee.Birthday;
            if (updateEmployee.Node != null) employee.Node = updateEmployee.Node;
            int check = await APIConnecter.SQL.SqlData.SaveChangesAsync();
            Employee result = await APIConnecter.SQL.SqlData.Employees.FindAsync(employeeId);
            if (check > 0) return new Result
            {
                status = Status.OK,
                data = result
            };
            else return new Result
            {
                status = Status.BadRequest,
                data = Messages.BadRequest
            };
        }

        public Result DeleteEmployee(int employeeId)
        {
            Employee employee = APIConnecter.SQL.SqlData.Employees.Find(employeeId);
            if (employee == null) return new Result
            {
                status = Status.BadRequest,
                data = Messages.BadRequest
            };
            APIConnecter.SQL.SqlData.Remove(employee);
            int check = APIConnecter.SQL.SqlData.SaveChanges();
            if (check > 0) return new Result
            {
                status = Status.OK,
                data = employee
            };
            else return new Result
            {
                status = Status.BadRequest,
                data = Messages.BadRequest
            };
        }

        public async Task<Result> DeleteEmployeeAsync(int employeeId)
        {
            Employee employee = await APIConnecter.SQL.SqlData.Employees.FindAsync(employeeId);
            if (employee == null) return new Result
            {
                status = Status.BadRequest,
                data = Messages.BadRequest
            };
            APIConnecter.SQL.SqlData.Remove(employee);
            int check = await APIConnecter.SQL.SqlData.SaveChangesAsync();
            if (check > 0) return new Result
            {
                status = Status.OK,
                data = employee
            };
            else return new Result
            {
                status = Status.BadRequest,
                data = Messages.BadRequest
            };
        }
    }
}
