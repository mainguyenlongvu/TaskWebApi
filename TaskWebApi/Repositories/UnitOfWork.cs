using System;
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
        private IUserRepository _userRepository;
        private IRefreshTokensRepository _refreshTokensRepository;
        public UnitOfWork(TaskDbContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context);
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

