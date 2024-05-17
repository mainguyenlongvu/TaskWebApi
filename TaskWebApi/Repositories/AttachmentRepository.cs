using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TaskWebApi.Data.Entities;
using TaskWebApi.Model;
using TaskWebApi.Repositories.EF;
using TaskWebApi.Repositories.Entities;

namespace TaskWebApi.Repositories
{
    public interface IAttachmentRepository
    {
        Task<List<AttachmentModel>?> AddAttachments(ICollection<IFormFile> files, string applicationId);
        Task<List<AttachmentModel>> GetAllAttachment();
        Task<string?> DeletePhoto(string publicId);
    }

    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly string _imageContentFolder;
        private readonly IMapper _mapper;
        private readonly TaskDbContext _context;

        public AttachmentRepository(TaskDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

            // Đường dẫn tĩnh để lưu trữ tệp
            _imageContentFolder = @"D:\FPTDocument\Summer2024\OJT\Project\TaskWebApi\TaskWebApi\Img_file";

            // Kiểm tra và tạo thư mục nếu nó không tồn tại
            if (!Directory.Exists(_imageContentFolder))
            {
                Directory.CreateDirectory(_imageContentFolder);
            }
        }

        public async Task<List<AttachmentModel>> GetAllAttachment()
        {
            var application = await _context.Attachments
                 .ProjectTo<AttachmentModel>(_mapper.ConfigurationProvider)
                 .ToListAsync();

            return application;

        }


        public async Task<List<AttachmentModel>?> AddAttachments(ICollection<IFormFile> files, string applicationId)
        {
            var attachmentModels = new List<AttachmentModel>();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var uniqueFileName = UploadedFile(file);
                    var filePath = Path.Combine(_imageContentFolder, uniqueFileName);

                    var attachment = new AttachmentEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        Url = filePath,
                        UploadedAt = DateTime.Now,
                        applicationId = applicationId
                    };

                    await _context.Attachments.AddAsync(attachment);
                    await _context.SaveChangesAsync();

                    var attachmentModel = _mapper.Map<AttachmentModel>(attachment);
                    attachmentModels.Add(attachmentModel);
                }
            }

            return attachmentModels.Count > 0 ? attachmentModels : null;
        }

        private string UploadedFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(_imageContentFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return uniqueFileName;
        }

        public void DeleteImage(string fileName)
        {
            var filePath = Path.Combine(_imageContentFolder, fileName);
            if (System.IO.File.Exists(filePath))
            {
                Task.Run(() => System.IO.File.Delete(filePath));
            }
        }

        public Task<string?> DeletePhoto(string publicId)
        {
            DeleteImage(publicId);
            return Task.FromResult<string?>(publicId);
        }
    }
}
