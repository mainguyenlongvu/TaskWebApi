using AutoMapper;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.SqlServer.Server;
using System.Net.Mail;
using System;
using TaskWebApi.Model;
using TaskWebApi.Repositories;
using TaskWebApi.Repositories.Entities;
using ApplicationModel = TaskWebApi.Model.ApplicationModel;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Mvc;

namespace TaskWebApi.Services
{
    public interface IApplicationService
    {
        Task<ActionResult<ApplicationModel>> SentApplication(ApplicationModel applicationModel, ICollection<IFormFile> files);
        Task<ApplicationModel?> ApproveApplication(string applicationId);
        Task<ApplicationModel?> RejectApplication(string applicationId);
        Task<List<ApplicationModel>> GetAllApplication();
        Task<ApplicationModel> GetApplicationById(string applicationId);
        Task<ApplicationEntity> DeleteApplication(string applicationId, string attachmentId);

    }
    public class ApplicationService : IApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService _attachmentService;

        public ApplicationService(IUnitOfWork unitOfWork, IMapper mapper, IAttachmentService attachmentService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _attachmentService = attachmentService;
        }
        public async Task<ApplicationModel?> ApproveApplication(string applicationId)
        {
            var applicationRepository = _unitOfWork.ApplicationRepository;
            var result = await applicationRepository.ApproveApplication(applicationId);
            return result;
        }

        public async Task<ApplicationEntity> DeleteApplication(string applicationId, string attachmentId)
        {
            var applicationRepository = _unitOfWork.ApplicationRepository;
            var result = await applicationRepository.DeleteApplication(applicationId, attachmentId);
            return result;
        }

        public async Task<List<ApplicationModel>> GetAllApplication()
        {
            var applicationRepository = _unitOfWork.ApplicationRepository;
            var result = await applicationRepository.GetAllApplication();
            return result;
        }

        public async Task<ApplicationModel> GetApplicationById(string applicationId)
        {
            var applicationRepository = _unitOfWork.ApplicationRepository;
            var result = await applicationRepository.GetApplicationById(applicationId);
            return result;
        }


        public async Task<ApplicationModel?> RejectApplication(string applicationId)
        {
            var applicationRepository = _unitOfWork.ApplicationRepository;
            var result = await applicationRepository.RejectApplication(applicationId);
            return result;
        }

        public async Task<ActionResult<ApplicationModel>> SentApplication(ApplicationModel applicationModel, ICollection<IFormFile> files)
        {
           
                var result = await _unitOfWork.ApplicationRepository.SentApplication(applicationModel, files);
                return result;
            
        }

    }
}
