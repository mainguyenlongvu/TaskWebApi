using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskWebApi.Repositories.Entities
{
    [Table("UserClaim")]

    public class UserClaimEntity
    {
        [Key]
        public string Id { get; set; }
        [ForeignKey(nameof(UserEntity))]
        public string UserId { get; set; }
        public UserEntity User { get; set; }
        [ForeignKey(nameof(ClaimEntity))]
        public string ClaimId { get; set; }
        public ClaimEntity Claim { get; set; }
        public string? ClaimType { get; set; }

        public string? ClaimValue { get; set; }
    }
}
