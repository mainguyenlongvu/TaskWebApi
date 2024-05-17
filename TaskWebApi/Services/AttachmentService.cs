using AutoMapper;
using TaskWebApi.Model;
using TaskWebApi.Repositories;

namespace TaskWebApi.Services
{
    public interface IAttachmentService
    {
        Task<List<AttachmentModel>> GetAllAttachment();
    }
    public class AttachmentService : IAttachmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttachmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<AttachmentModel>> GetAllAttachment()
        {
            var attachmentRepository = _unitOfWork.AttachmentRepository;
            var result = await attachmentRepository.GetAllAttachment();
            return result;
        }
    }
}
