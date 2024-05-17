using System.ComponentModel.DataAnnotations.Schema;
using TaskWebApi.Enum;
using TaskWebApi.Repositories.Entities;

namespace TaskWebApi.Model
{
    public class WageModel
    {
        public double Price { get; set; }
        public double Reward { get; set; }
        public string UserId { get; set; }
    }
}
