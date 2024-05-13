using TaskWebApi.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskWebApi.Repositories.Configurations
{
    public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaimEntity>
    {
        public void Configure(EntityTypeBuilder<UserClaimEntity> builder)
        {
            builder.ToTable("UserClaim");

            // Primary Key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.ClaimId);

            // Foreign Keys
            builder.HasOne(bp => bp.User)
                .WithMany(b => b.UserClaims)
                .HasForeignKey(bp => bp.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(bp => bp.Claim)
                .WithMany(p => p.UserClaims)
                .HasForeignKey(bp => bp.ClaimId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
