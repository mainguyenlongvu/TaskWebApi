﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskWebApi.Enum;
namespace TaskWebApi.Repositories.Entities
{
    [Table("Wage")]

    public class WageEntity
    {
        [Key]

        public int Id { get; set; }
        public WagetType Type { get; set; }
        public double Price { get; set; }
        public int DayOff { get; set; }
        public int Total { get; set; }
        public int UserId { get; set; }
        public UserEntity UserEntity { get; set; }
    }
}
