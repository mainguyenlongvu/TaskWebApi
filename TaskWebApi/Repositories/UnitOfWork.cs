using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using TaskWebApi.CoreHelper.Helper;
using TaskWebApi.Model;
using TaskWebApi.Repositories.EF;
using TaskWebApi.Repositories.Entities;

namespace TaskWebApi.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IRefreshTokensRepository RefreshTokensRepository { get; }
        IApplicationRepository ApplicationRepository { get;  }
        IAttachmentRepository AttachmentRepository { get; }
        IWageRepository WageRepository { get; }
        Task<int> SaveChangesAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskDbContext _context;
        private IOptionsMonitor<AppSetting> _optionsMonitor;
        private IUserRepository _userRepository;
        private IRefreshTokensRepository _refreshTokensRepository;
        private IApplicationRepository _applicationRepository;
        private IAttachmentRepository _attachmentRepository;
        private IWageRepository _wageRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        //private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UserEntity> _userManager;
        private readonly SendMail _sendMail;
        public UnitOfWork(TaskDbContext context,  IOptionsMonitor<AppSetting> optionsMonitor, IConfiguration configuration, SignInManager<UserEntity> signInManager, RoleManager<IdentityRole> roleManager, UserManager<UserEntity> userManager, SendMail sendMail, IApplicationRepository applicationRepository, IAttachmentRepository attachmentRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment)  {
            _context = context;
            _optionsMonitor = optionsMonitor;
            _configuration = configuration;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
            _sendMail = sendMail;
            _applicationRepository = applicationRepository;
            _attachmentRepository = attachmentRepository;
            _mapper = mapper;
            //_webHostEnvironment = webHostEnvironment;
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context, _optionsMonitor, _configuration, _roleManager, _userManager, _signInManager, _sendMail);
                }
                return _userRepository;
            }
        }

        public IWageRepository WageRepository
        {
            get
            {
                if (_wageRepository == null)
                {
                    _wageRepository = new WageRepository(_context, _mapper, _userManager);
                }
                return _wageRepository;
            }
        }

        public IRefreshTokensRepository RefreshTokensRepository
        {
            get
            {
                if (_refreshTokensRepository == null)
                {
                    _refreshTokensRepository = new RefreshTokensRepository(_context);
                }
                return _refreshTokensRepository;
            }
        }

        public IApplicationRepository ApplicationRepository
        {
            get
            {
                if (_applicationRepository == null)
                {
                    _applicationRepository = new ApplicationRepository(_context, _mapper, _attachmentRepository);
                }
                return _applicationRepository;
            }
        }

        public IAttachmentRepository AttachmentRepository
        {
            get
            {
                if (_attachmentRepository == null)
                {
                    _attachmentRepository = new AttachmentRepository(_context, _mapper);
                }
                return _attachmentRepository;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

