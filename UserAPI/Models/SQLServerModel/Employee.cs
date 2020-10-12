using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAPI.Models.SQLServerModel
{
    [Table("Employee")]
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Username { get; set; }

        [StringLength(200)]
        public string Password { get; set; }

        [StringLength(200)]
        public string Image { get; set; }

        public DateTime? Birthday { get; set; }

        [StringLength(10)]
        public string Sex { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(200)]
        public string Position { get; set; }

        [StringLength(100)]
        public string Node { get; set; }
    }

    public class InsertEmployeeInfo
    {
        [Required(ErrorMessage = "the name is required")]
        [StringLength(200)]
        public string Name { get; set; }

        [Required(ErrorMessage = "the username is required")]
        [StringLength(200)]
        public string Username { get; set; }

        [Required(ErrorMessage = "the password is required")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Minimum eight characters, at least one letter and one number")]
        [StringLength(200)]
        public string Password { get; set; }

        [StringLength(200)]
        public string Image { get; set; }

        public DateTime? Birthday { get; set; }

        [Required(ErrorMessage = "the sex is required")]
        [StringLength(10)]
        public string Sex { get; set; }

        [Required(ErrorMessage = "the phone is required")]
        [StringLength(50)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "the address is required")]
        [StringLength(200)]
        public string Address { get; set; }

        [Required(ErrorMessage = "the position is required")]
        [StringLength(200)]
        public string Position { get; set; }

        [StringLength(100)]
        public string Node { get; set; }
    }
}
