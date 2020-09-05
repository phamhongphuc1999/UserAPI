using Microsoft.EntityFrameworkCore;
using MongoDatabase.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserAPI.Models.SQLServer
{
    public class EmployeeDbContext: DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {

        }

        public async Task<Result> GetListEmployees()
        {
            List<Employee> employeeList = Employees.Local.ToList<Employee>();
            return new Result
            {
                status = 200,
                data = employeeList
            };
        }
    }
}
