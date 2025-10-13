using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Identity.Data.EntityConfigurations;
internal class ApplicationClaimConfiguration : IEntityTypeConfiguration<ApplicationClaim>
{
    public void Configure(EntityTypeBuilder<ApplicationClaim> builder)
    {
        builder.ToTable("Claims");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Type).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Value).HasMaxLength(500).IsRequired();
    }
}
