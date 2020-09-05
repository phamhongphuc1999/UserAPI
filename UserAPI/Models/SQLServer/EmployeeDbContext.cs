using Microsoft.EntityFrameworkCore;

namespace UserAPI.Models.SQLServer
{
    public class EmployeeDbContext: DbContext
    {
        public DbSet<Employee> employee { get; set; }

        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {

        }
    }
}
