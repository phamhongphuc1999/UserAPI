using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace UserAPI.Models.MongoModel
{
  public class User
  {
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string id { get; set; }

    public string username { get; set; }

    public string password { internal get; set; }

    public string email { get; set; }

    public DateTime createAt { get; set; }

    public DateTime updateAt { get; set; }

    public DateTime lastLogin { get; set; }

    public bool status { get; set; }
  }

  public class NewUserInfo
  {
    [Required(ErrorMessage = "Username is required", AllowEmptyStrings = false)]
    [StringLength(200)]
    public string username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Minimum eight characters, at least one letter and one number")]
    [StringLength(200)]
    public string password { get; set; }

    [EmailAddress]
    public string email { get; set; }
  }

  public class UpdateUserInfo
  {
    [StringLength(200)]
    public string username { get; set; }

    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Minimum eight characters, at least one letter and one number")]
    [StringLength(200)]
    public string password { get; set; }

    [EmailAddress]
    public string email { get; set; }

    public DateTime updateAt { get; set; }

    public DateTime lastLogin { get; set; }
  }

  public class UserLoginInfo
  {
    [Required(ErrorMessage = "Username is required")]
    public string username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string password { get; set; }
  }

  public class HelperTokenUser
  {
    public string userId { get; set; }
    public string username { get; set; }
    public string email { get; set; }
  }
}
