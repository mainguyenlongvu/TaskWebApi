using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;
using TaskWebApi.Data.Entities;
using TaskWebApi.Enum;

namespace TaskWebApi.Repositories.Entities
{
    [Table("Application")]

    public class ApplicationEntity
    {
        [Key]
        public string Id { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey(nameof(UserEntity))]
        public string UserId { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime? RejectedAt { get; set; }
        public UserEntity? User { get; set; }
        public ICollection<AttachmentEntity> Attachments { get; set; }
    }
}
