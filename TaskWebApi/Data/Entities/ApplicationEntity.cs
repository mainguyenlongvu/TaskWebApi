using TaskWebApi.Enum;

namespace TaskWebApi.Repositories.Entities
{
    public class ApplicationEntity
    {
        public int Id { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public string Name { get; set; }
        public string reason { get; set; }
        public string Description { get; set; }
        public string? Image_Url { get; set; }
        public int UserId { get; set; }
        public bool IsApproved { get; set; }

        public bool IsRejected { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public DateTime? RejectedAt { get; set; }
        public UserEntity? User { get; set; }
    }
}
