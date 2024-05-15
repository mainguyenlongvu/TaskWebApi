using TaskWebApi.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using TaskWebApi.Repositories.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace TaskWebApi.Repositories.EF
{
    public class TaskDbContext : IdentityDbContext<UserEntity>
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> opt) : base(opt)
        {

        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    //Configure using Fluent API
        //    modelBuilder.ApplyConfiguration(new UserConfiguration());
        //    modelBuilder.ApplyConfiguration(new ApplicationConfiguration());
        //    modelBuilder.ApplyConfiguration(new ClaimConfiguration());
        //    modelBuilder.ApplyConfiguration(new RefreshTokensConfiguration());

        //    modelBuilder.ApplyConfiguration(new UserClaimConfiguration());
        //    modelBuilder.ApplyConfiguration(new WageConfiguration());

        //    // Generate data
        //}




        public DbSet<ApplicationEntity> Applications { get; set; }
        public DbSet<ClaimEntity> Claims { get; set; }
        public DbSet<WageEntity> Wages { get; set; }
        public DbSet<UserClaimEntity> UserClaims { get; set; }
        public DbSet<RefreshTokens> refreshTokens { get; set; }


    }
}
