using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NextGen.Modules.Identity.Identity.Data.EntityConfigurations;
internal class ModuleConfiguration : IEntityTypeConfiguration<NextGen.Modules.Identity.Shared.Models.ApplicationModule>
{
    public void Configure(EntityTypeBuilder<NextGen.Modules.Identity.Shared.Models.ApplicationModule> builder)
    {
        builder.ToTable("Modules");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Name).HasMaxLength(200).IsRequired();
    }
}
