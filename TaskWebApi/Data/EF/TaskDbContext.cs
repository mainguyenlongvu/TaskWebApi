using TaskWebApi.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using TaskWebApi.Repositories.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace TaskWebApi.Repositories.EF
{
    public class TaskDbContext : IdentityDbContext<UserEntity>
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> opt) : base(opt)
        {

        }


     

        public DbSet<ApplicationEntity> Applications { get; set; }
        public DbSet<ClaimEntity> Claims { get; set; }
        public DbSet<WageEntity> Wages { get; set; }
        public DbSet<UserClaimEntity> UserClaims { get; set; }
        public DbSet<RefreshTokens> refreshTokens { get; set; }


    }
}
