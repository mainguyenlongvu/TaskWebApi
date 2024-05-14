using System.ComponentModel.DataAnnotations;
using TaskWebApi.Enum;

namespace TaskWebApi.Model
{
    public class RegisterModel
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public Roles Roles { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; } 

    }
}
