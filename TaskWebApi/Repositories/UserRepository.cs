using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskWebApi.Enum;
using TaskWebApi.Model;
using TaskWebApi.Repositories.EF;
using TaskWebApi.Repositories.Entities;

namespace TaskWebApi.Repositories
{
    public interface IUserRepository
    {
        Task<List<UserEntity>> GetAllAsync();
        Task<UserEntity> GetByIdAsync(string id);
        Task<UserEntity> GetByEmailAsync(string email);

        Task<UserEntity> CreateAsync(UserEntity entity);

        Task<ResponseLoginModel> CheckLoginAsync(RequestLoginModel entity);
        Task DeleteAsync(string id);
        Task<UserEntity> UpdateAsync(UserEntity entity);
    }

    public class UserRepository : IUserRepository
    {
        private readonly TaskDbContext _context;
        private readonly AppSetting _appSettings;
        private readonly IConfiguration _configuration;

        public UserRepository(TaskDbContext context, IOptionsMonitor<AppSetting> optionsMonitor, IConfiguration configuration)
        {
            _context = context;
            _appSettings = optionsMonitor.CurrentValue;
            _configuration = configuration;
        }

        public async Task<List<UserEntity>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<UserEntity> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<UserEntity> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<UserEntity> CreateAsync(UserEntity entity)
        {
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<ResponseLoginModel> CheckLoginAsync(RequestLoginModel entity)
        {
            var user= _context.Users.SingleOrDefault(x => x.Email == entity.Email && x.Password == entity.Password);
             if (user == null)
            {
                return new ResponseLoginModel
                {
                    Success = false,
                    Message = "Invalid username/password"
                };
            }
            return new ResponseLoginModel
            {
                FullName = user.Name,
                UserId = user.Id,
                Token= GenerateToken(user),
                Success = true,
                Message = "Authenticate success",

            };
        }
        private string GenerateToken(UserEntity userEntity)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
           
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, userEntity.Name),
                    new Claim(ClaimTypes.Email, userEntity.Email),
                    new Claim("UserName", userEntity.Email),
                    new Claim("Id", userEntity.Id.ToString()),
                    new Claim(ClaimTypes.Role, userEntity.Roles.ToString()),
                    //roles

                    new Claim("TokenId", Guid.NewGuid().ToString())
                }),
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            return jwtTokenHandler.WriteToken(token);
        }

        public async Task DeleteAsync(string id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Users.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<UserEntity> UpdateAsync(UserEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}

