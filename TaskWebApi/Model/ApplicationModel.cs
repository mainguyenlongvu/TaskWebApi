using TaskWebApi.Enum;

namespace TaskWebApi.Model
{
    public class ApplicationModel
    {
        public int Id { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public string Name { get; set; }
        public string reason { get; set; }
        public string Description { get; set; }
        public string JobPosition { get; set; }
        public string? Image_Url { get; set; }
        public IFormFile? File { get; set; }
    }
}
