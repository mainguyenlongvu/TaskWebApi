﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskWebApi.Repositories.Entities
{
    [Table("UserClaim")]

    public class UserClaimEntity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(UserEntity))]
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        [ForeignKey(nameof(ClaimEntity))]
        public int ClaimId { get; set; }
        public ClaimEntity Claim { get; set; }
        public string? ClaimType { get; set; }

        public string? ClaimValue { get; set; }
    }
}
