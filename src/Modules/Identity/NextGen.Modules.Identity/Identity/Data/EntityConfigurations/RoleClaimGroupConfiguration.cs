using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Identity.Data.EntityConfigurations;

public class RoleClaimGroupConfiguration : IEntityTypeConfiguration<RoleClaimGroup>
{
    public void Configure(EntityTypeBuilder<RoleClaimGroup> builder)
    {
        builder.ToTable("RoleClaimGroups");

        //builder.HasKey(rcg => rcg.Id);

        //builder.Property(rcg => rcg.Id)
        //    .HasDefaultValueSql("NEWID()");

        builder.Property(rcg => rcg.RoleId)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(rcg => rcg.ClaimGroupId)
            .IsRequired();

        builder.HasOne(rcg => rcg.Role)
            .WithMany()
            .HasForeignKey(rcg => rcg.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rcg => rcg.ClaimGroup)
            .WithMany()
            .HasForeignKey(rcg => rcg.ClaimGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
