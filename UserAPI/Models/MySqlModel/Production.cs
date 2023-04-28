using System.ComponentModel.DataAnnotations;
using UserAPI.Configuration;

namespace UserAPI.Models.MySqlModel
{
  [Table("Productions")]
  public class Production
  {
    public int Id { get; set; }

    [StringLength(30)]
    public string Name { get; set; }

    public int Amount { get; set; }

    public int employeeId { get; set; }
  }
}