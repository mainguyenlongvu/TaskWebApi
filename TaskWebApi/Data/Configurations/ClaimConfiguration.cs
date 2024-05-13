using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskWebApi.Repositories.Entities;

namespace TaskWebApi.Repositories.Configurations
{
    public class ClaimConfiguration : IEntityTypeConfiguration<ClaimEntity>
    {
        public void Configure(EntityTypeBuilder<ClaimEntity> builder)
        {
            builder.ToTable("Claim");

            // Primary Key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            // Other properties


            
           


        }
    }
}
