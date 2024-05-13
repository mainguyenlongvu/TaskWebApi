using TaskWebApi.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using TaskWebApi.Repositories.Configurations;

namespace TaskWebApi.Repositories.EF
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure using Fluent API
            modelBuilder.ApplyConfiguration(new ApplicationConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new WageConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.ApplyConfiguration(new UserClaimConfiguration());

            // Generate data
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ApplicationEntity> Applications { get; set; }
        public DbSet<ClaimEntity> Claims { get; set; }
        public DbSet<WageEntity> Wages { get; set; }
        public DbSet<UserClaimEntity> UserClaims { get; set; }
        public DbSet<RefreshTokens> refreshTokens { get; set; }


    }
}
