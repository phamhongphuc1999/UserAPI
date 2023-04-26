using Microsoft.EntityFrameworkCore;

namespace UserAPI.Models.SQLServerModel
{
  public class SQLData : DbContext
  {
    public DbSet<Employee> Employees { get; set; }

    public SQLData(DbContextOptions<SQLData> options) : base(options)
    {
    }
  }
}
