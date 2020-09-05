using Microsoft.EntityFrameworkCore;

namespace UserAPI.Models.SQLServer
{
    public class SQLData: DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public SQLData(DbContextOptions<SQLData> options) : base(options)
        {
        }
    }
}
