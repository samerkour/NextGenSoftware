using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextGen.Modules.Identity.Shared.Models;


namespace NextGen.Modules.Identity.Identity.Data.EntityConfigurations;

public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.ToTable("RoleClaims");

        builder.HasKey(rc => new { rc.RoleId, rc.ClaimId });

        builder.Property(rc => rc.RoleId)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(rc => rc.ClaimId)
            .IsRequired();

        builder.HasOne(rc => rc.Role)
            .WithMany()
            .HasForeignKey(rc => rc.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rc => rc.Claim)
            .WithMany()
            .HasForeignKey(rc => rc.ClaimId)
            .OnDelete(DeleteBehavior.NoAction); // matches your SQL (no cascade)
    }
}
