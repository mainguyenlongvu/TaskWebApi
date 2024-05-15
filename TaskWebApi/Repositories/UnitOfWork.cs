using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using TaskWebApi.Model;
using TaskWebApi.Repositories.EF;
using TaskWebApi.Repositories.Entities;

namespace TaskWebApi.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IRefreshTokensRepository RefreshTokensRepository { get; }
        Task<int> SaveChangesAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskDbContext _context;
        private IOptionsMonitor<AppSetting> _optionsMonitor;
        private IUserRepository _userRepository;
        private IRefreshTokensRepository _refreshTokensRepository;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UserEntity> _userManager;

        public UnitOfWork(TaskDbContext context,  IOptionsMonitor<AppSetting> optionsMonitor, IConfiguration configuration, SignInManager<UserEntity> signInManager, RoleManager<IdentityRole> roleManager, UserManager<UserEntity> userManager)
        {
            _context = context;
            _optionsMonitor = optionsMonitor;
            _configuration = configuration;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context, _optionsMonitor, _configuration, _roleManager, _userManager, _signInManager);
                }
                return _userRepository;
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

