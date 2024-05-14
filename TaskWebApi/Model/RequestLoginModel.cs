using System.ComponentModel.DataAnnotations;

namespace TaskWebApi.Model
{
    public class RequestLoginModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
