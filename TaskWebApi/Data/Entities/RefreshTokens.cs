using System.ComponentModel.DataAnnotations.Schema;
using TaskWebApi.Repositories.Entities;

namespace TaskWebApi.Repositories.Entities
{
    public class RefreshTokens
    {
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }
        public int UserId { get; set; }

        public string? Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsActive { get; set; }
    }
}
