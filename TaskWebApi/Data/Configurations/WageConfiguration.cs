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
    public class WageConfiguration : IEntityTypeConfiguration<WageEntity>
    {
        public void Configure(EntityTypeBuilder<WageEntity> builder)
        {
            builder.ToTable("Wage");

            // Primary Key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            // Other properties

            builder.Property(x => x.UserId).IsRequired();

            // 1:M relationship with User
            builder.HasOne(b => b.UserEntity)
                   .WithMany(u => u.Wages)
                   .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.NoAction);
           


        }
    }
}
