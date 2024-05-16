using TaskWebApi.Data.Entities;
using TaskWebApi.Enum;

namespace TaskWebApi.Model
{
    public class ApplicationModel
    {
        public string Id { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string JobPosition { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public required string UserId { get; set; }
        //public ICollection<AttachmentModel> Attachments { get; set; }
    }
}
