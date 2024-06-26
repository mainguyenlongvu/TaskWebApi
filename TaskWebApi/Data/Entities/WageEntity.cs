﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskWebApi.Enum;
namespace TaskWebApi.Repositories.Entities
{
    [Table("Wage")]

    public class WageEntity
    {
        [Key]

        public string Id { get; set; }
        public double Price { get; set; }
        public int DayOffApproved { get; set; }
        public int DayOffRejected { get; set; }
        public double Total { get; set; }
        [ForeignKey(nameof(UserEntity))]
        public string UserId { get; set; }
        public UserEntity User { get; set; }
    }
}
