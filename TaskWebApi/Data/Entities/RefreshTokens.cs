﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskWebApi.Repositories.Entities;

namespace TaskWebApi.Repositories.Entities
{
    [Table("RefreshToken")]

    public class RefreshTokens
    {
        [Key]
        public string Id { get; set; }

        
        public UserEntity User { get; set; }
        [ForeignKey(nameof(UserEntity))]
        public string UserId { get; set; }

        public string? Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsActive { get; set; }
    }
}
