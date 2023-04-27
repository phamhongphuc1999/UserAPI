using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAPI.Models.MySqlModel
{
  [Table("Employees")]
  public class Employee
  {
    public int Id { get; set; }

    [StringLength(30)]
    public string Username { get; set; }

    [StringLength(100)]
    public string Password { get; set; }

    [StringLength(50)]
    public string Email { get; set; }
  }

  public class InsertEmployeeInfo
  {
    [Required(ErrorMessage = "Username is required")]
    [StringLength(30)]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Minimum eight characters, at least one letter and one number")]
    [StringLength(100)]
    public string Password { get; set; }

    [EmailAddress]
    public string Email { get; set; }
  }
}
