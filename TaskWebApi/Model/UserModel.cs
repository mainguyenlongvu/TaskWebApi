using TaskWebApi.Enum;

namespace TaskWebApi.Model
{
    public class UserModel
    {
        public int Id { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public Roles Roles { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
