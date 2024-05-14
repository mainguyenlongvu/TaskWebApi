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
    public class ApplicationConfiguration : IEntityTypeConfiguration<ApplicationEntity>
    {
        public void Configure(EntityTypeBuilder<ApplicationEntity> builder)
        {
            builder.ToTable("Application");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Other properties

            builder.Property(x => x.Name).IsUnicode().IsRequired();
            builder.Property(x => x.UserId).IsRequired();

            // 1:M relationship with User
            builder.HasOne(b => b.User)
                   .WithMany(u => u.Applications)
                   .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.NoAction);


        }

    }
}
