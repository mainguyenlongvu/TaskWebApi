using TaskWebApi.Enum;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskWebApi.Repositories.Entities;
using Microsoft.AspNetCore.Identity;

namespace TaskWebApi.Repositories.Entities
{
    [Table("User")]
    public class UserEntity : IdentityUser
    {
        

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string? VerificationToken { get; set; }
        public string? ResetToken { get; set; }
        public DateTime ResetTokenExpire { get; set; }
        public string Email { get; set; }
        [DefaultValue("true")]
        public bool IsActive { get; set; }
        public Roles Roles { get; set; }

        public virtual ICollection<RefreshTokens> RefreshTokens { get; set; }
        public List<ApplicationEntity> Applications { get; set; }
        public List<WageEntity> Wages { get; set; }
        public List<UserClaimEntity> UserClaims { get; set; }
    }
}
