using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskWebApi.Model;
using TaskWebApi.Repositories.EF;
using TaskWebApi.Repositories.Entities;

namespace TaskWebApi.Repositories
{
    public interface IRefreshTokensRepository
    {
        Task<RefreshTokens>? AddRefreshTokensAsync(RefreshTokens entity);
    }
    public class RefreshTokensRepository: IRefreshTokensRepository
    {
        private readonly TaskDbContext _context;

        public RefreshTokensRepository(TaskDbContext context)
        {
            _context = context;
        }
        public async Task<RefreshTokens>? AddRefreshTokensAsync(RefreshTokens entity)
        {
            _context.refreshTokens.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
