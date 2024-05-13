using System.ComponentModel.DataAnnotations;

namespace TaskWebApi.Repositories.Entities
{
    public abstract class Entity
    {
        protected Entity()
        {
            //Id = Guid.NewGuid().ToString("N");
            CreateTimes = DateTime.Now;
            //LastUpdateTimes = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }
        public DateTime CreateTimes { get; set; }
        //public DateTime LastUpdateTimes { get; set; }
        //public DateTime CreateUser { get; set; }
        //public DateTime LastUpdateUser { get; set; }
    }
}
