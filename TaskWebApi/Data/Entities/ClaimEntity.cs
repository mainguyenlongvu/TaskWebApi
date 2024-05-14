using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskWebApi.Repositories.Entities
{
    [Table("Claim")]

    public class ClaimEntity
    {
        [Key]

        public int Id { get; set; }
        public List<UserClaimEntity> UserClaims { get; set; }

    }
}
