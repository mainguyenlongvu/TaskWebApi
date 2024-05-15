using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
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
using TaskWebApi.CoreHelper.Helper;
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

        Task<IdentityResult> CreateAsync(RegisterModel model);

        Task<string> CheckLoginAsync(RequestLoginModel entity);
        Task DeleteAsync(string id);
        Task<UserEntity> UpdateAsync(UserEntity entity);
    }

    public class UserRepository : IUserRepository
    {
        private readonly TaskDbContext _context;
        private readonly AppSetting _appSettings;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UserEntity> _userManager;

        private readonly SignInManager<UserEntity> _signInManager;


        public UserRepository(TaskDbContext context, IOptionsMonitor<AppSetting> optionsMonitor, IConfiguration configuration, RoleManager<IdentityRole> roleManager, UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager)
        {
            _context = context;
            _appSettings = optionsMonitor.CurrentValue;
            _configuration = configuration;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
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

        public async Task<IdentityResult> CreateAsync(RegisterModel model)
        {
            var user = new UserEntity
            {
                Name = model.Name,
                DateOfBirth = model.DateOfBirth,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                Roles = model.Roles,
                IsActive = true // Default value for IsActive
            };

            // Create user with UserManager
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Ensure the Admin role exists
                if (!await _roleManager.RoleExistsAsync(AppRole.Admin))
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(AppRole.Admin ));
                    if (!roleResult.Succeeded)
                    {
                        return IdentityResult.Failed(new IdentityError { Description = "Failed to create admin role" });
                    }
                }

                // Assign the Admin role to the user
                var addToRoleResult = await _userManager.AddToRoleAsync(user, AppRole.Admin);
                if (!addToRoleResult.Succeeded)
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Failed to add user to admin role" });
                }
            }

            return result;
        }


        public async Task<string> CheckLoginAsync(RequestLoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return string.Empty;
            }

            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Email, model.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(20),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
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

