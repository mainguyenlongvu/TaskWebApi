using TaskWebApi.Enum;
namespace TaskWebApi.Repositories.Entities
{
    public class WageEntity
    {
        public int Id { get; set; }
        public WagetType Type { get; set; }
        public double Price { get; set; }
        public int DayOff { get; set; }
        public int Total { get; set; }
        public int UserId { get; set; }
        public UserEntity UserEntity { get; set; }
    }
}
