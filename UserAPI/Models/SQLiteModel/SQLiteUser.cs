using System.ComponentModel.DataAnnotations;

namespace UserAPI.Models.SQLiteModel
{
  public class SQLiteUser
  {
    public int id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
  }

  public class LoginUser
  {
    [Required(ErrorMessage = "the username is required")]
    public string Username { get; set; }

    [Required(ErrorMessage = "the password is required")]
    public string Password { get; set; }
  }

  public class HelperSQLiteTokenUser
  {
    public string userId { get; set; }
    public string Username { get; set; }
  }
}
