using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MongoDatabase.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string username { get; set; }

        public string password { internal get; set; }

        public string name { get; set; }

        public string location { get; set; }

        public string email { get; set; }

        public string birthday { get; set; }

        public string phone { get; set; }

        public string role { get; set; }

        public string createAt { get; set; }

        public string updateAt { get; set; }

        public string lastLogin { get; set; }

        public string status { get; set; }
    }

    public class NewUserInfo
    {
        [Required(ErrorMessage = "the username is required", AllowEmptyStrings = false)]
        [StringLength(200)]
        public string username { get; set; }

        [Required(ErrorMessage = "the password is required")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Minimum eight characters, at least one letter and one number")]
        [StringLength(200)]
        public string password { get; set; }

        [Required(ErrorMessage = "the name is required", AllowEmptyStrings = false)]
        public string name { get; set; }

        [Required(ErrorMessage = "the location is required")]
        public string location { get; set; }

        [Required(ErrorMessage = "the email is required")]
        [EmailAddress]
        public string email { get; set; }

        [Required(ErrorMessage = "the phone is required")]
        [Phone]
        public string phone { get; set; }

        public string birthday { get; set; }
    }

    public class UpdateUserInfo
    {
        [StringLength(200)]
        public string username { get; set; }

        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Minimum eight characters, at least one letter and one number")]
        [StringLength(200)]
        public string password { get; set; }
        
        public string name { get; set; }
        
        public string location { get; set; }
        
        [EmailAddress]
        public string email { get; set; }
        
        public string birthday { get; set; }
        
        [Phone]
        public string phone { get; set; }
    }

    public class UpdateRoleUserInfo
    {
        [IncludeArray(true, CheckArray = new object[] { "admin", "user", "customer"}, ErrorMessage = "role is one of admin, user and customer")]
        public string role { get; set; }
        
        [IncludeArray(true, CheckArray = new object[] { "enable", "disable"}, ErrorMessage = "status is enable or disable")]
        public string status { get; set; }
    }

    public class UserLoginInfo
    {
        [Required(ErrorMessage = "the username is required")]
        public string username { get; set; }

        [Required(ErrorMessage = "the password is required")]
        public string password { get; set; }
    }
}
