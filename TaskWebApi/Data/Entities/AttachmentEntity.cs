using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TaskWebApi.Repositories.Entities;

namespace TaskWebApi.Data.Entities
{

    [Table("Attachments")]
    public class AttachmentEntity
    {
        [Key]
        public required string Id { get; set; }

        [Required]
        public required string Url { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.Now;
        [ForeignKey(nameof(ApplicationEntity))]
        public required string applicationId { get; set; } 

        [ForeignKey("applicationId")]
        public virtual ApplicationEntity Applications { get; set; }  
    }
}


