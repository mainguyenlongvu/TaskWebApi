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
    public class RefreshTokensConfiguration : IEntityTypeConfiguration<RefreshTokens>
    {
        public void Configure(EntityTypeBuilder<RefreshTokens> builder)
        {
            builder.ToTable("RefreshTokens");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Other properties


            
           


        }
    }
}
