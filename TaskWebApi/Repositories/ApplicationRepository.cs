using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using TaskWebApi.Model;
using TaskWebApi.Repositories.EF;
using TaskWebApi.Repositories.Entities;
using TaskWebApi.Services;
using static System.Net.Mime.MediaTypeNames;

namespace TaskWebApi.Repositories
{
    public interface IApplicationRepository
    {
        Task<ActionResult<ApplicationModel>> SentApplication(ApplicationModel model, ICollection<IFormFile> files);
        Task<ApplicationModel?> ApproveApplication(string applicationId);
        Task<ApplicationModel?> RejectApplication(string applicationId);
        Task<List<ApplicationModel>> GetAllApplication();
        Task<ApplicationModel> GetApplicationById(string applicationId);
        Task<ApplicationEntity> DeleteApplication(string applicationId, string attachmentId);
    }
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly IMapper _mapper;
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly TaskDbContext _context;
        public ApplicationRepository(TaskDbContext context, IMapper mapper, IAttachmentRepository attachmentRepository)
        {
            _mapper = mapper;
            _context = context;
            _attachmentRepository = attachmentRepository;
        }

        public async Task<ActionResult<ApplicationModel>> SentApplication(ApplicationModel model, ICollection<IFormFile> files)
        {
            try
            {
                var applicationEntity = _mapper.Map<ApplicationEntity>(model);

                _context.Applications.Add(applicationEntity);
                await _context.SaveChangesAsync(); // Lưu ứng dụng trước để lấy Id

                var attachments = await _attachmentRepository.AddAttachments(files, applicationEntity.Id);

                var applicationModel = _mapper.Map<ApplicationModel>(applicationEntity);

                return applicationModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while sending application.", ex);
            }
        }



        public async Task<ApplicationModel?> ApproveApplication(string applicationId)
        {
            var application = await _context.Applications.FindAsync(applicationId);

            if (application == null)
            {
                throw new ArgumentException("Application not found");
            }

            if (application.IsApproved)
            {
                throw new InvalidOperationException("Application has already been approved");
            }

            if (application.IsRejected)
            {
                // Remove rejection status and rejected date
                application.IsRejected = false;
                application.RejectedAt = null;
            }

            application.IsApproved = true;
            application.ApprovedAt = DateTime.Now;

            _context.Applications.Update(application);

            await _context.SaveChangesAsync();

            var applicationModel = await _context.Applications
                .Where(f => f.Id == applicationId)
                .ProjectTo<ApplicationModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            return applicationModel;
        }

        public async Task<ApplicationModel?> RejectApplication(string applicationId)
        {
            var application = await _context.Applications.FindAsync(applicationId);

            if (application == null)
                throw new ArgumentException("Application not found");

            if (application.IsRejected)
                throw new InvalidOperationException("Application has already been rejected");

            if (application.IsApproved)
            {
                application.IsApproved = false;
                application.ApprovedAt = null;
            }

            application.IsRejected = true;
            application.RejectedAt = DateTime.Now;

            _context.Applications.Update(application);
            await _context.SaveChangesAsync();

            var applicationModel = await _context.Applications
                .Where(f => f.Id == applicationId)
                .ProjectTo<ApplicationModel>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            return applicationModel;
        }

        public async Task<List<ApplicationModel>> GetAllApplication()
        {
            var application = await _context.Applications
                 .ProjectTo<ApplicationModel>(_mapper.ConfigurationProvider)
                 .ToListAsync();

            return application;

        }

        public async Task<ApplicationModel> GetApplicationById(string applicationId)
        {
            var existingApplication = await _context.Applications.Where(f => f.Id == applicationId)
           .ProjectTo<ApplicationModel>(_mapper.ConfigurationProvider)
           .SingleOrDefaultAsync();

            if (existingApplication == null) throw new ArgumentException("Applications not found");

            return existingApplication;
        }

        public Task<ApplicationEntity> DeleteApplication(string applicationId, string attachmentId)
        {
            throw new NotImplementedException();
        }
    }
}

