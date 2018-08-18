using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DBPlatform_v1._0.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string password { get; set; }
        public string Name { get; set; }
        public string login { get; set; }
        public string Role { get; set; }

    }

    public class LoginModel
    {
        [Required]
        public string login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }



        [Required]
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }


       
    }
}