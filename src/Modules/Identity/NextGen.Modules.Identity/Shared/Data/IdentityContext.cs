using System.Data;
using BuildingBlocks.Abstractions.CQRS.Event.Internal;
using BuildingBlocks.Abstractions.Persistence;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NextGen.Modules.Identity.Shared.Models;

namespace NextGen.Modules.Identity.Shared.Data;

public class IdentityContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid,
        IdentityUserClaim<Guid>,
        ApplicationUserRole,
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>,
    IDbFacadeResolver,
    IDomainEventContext,
    ITxDbContextExecution
{
    public IdentityContext(DbContextOptions options) : base(options)
    {
    }

    // ----------------------------
    // DbSets
    // ----------------------------
    public DbSet<ApplicationModule> Modules { get; set; } = default!;
    public DbSet<RoleGroup> RoleGroups { get; set; } = default!;
    public DbSet<ClaimGroup> ClaimGroups { get; set; } = default!;
    public DbSet<ApplicationClaim> Claims { get; set; } = default!;
    public DbSet<ClaimGroupClaim> ClaimGroupClaims { get; set; } = default!;
    public DbSet<RoleClaimGroup> RoleClaimGroups { get; set; } = default!;
    public DbSet<RoleClaim> RoleClaims { get; set; } = default!;

    // ----------------------------
    // Model Configuration
    // ----------------------------
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        // ----------------------------
        // Naming convention: underscore_case
        // ----------------------------
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.GetTableName()?.Underscore());

            var objectIdentifier =
                StoreObjectIdentifier.Table(entity.GetTableName()?.Underscore()!, entity.GetSchema());

            foreach (var property in entity.GetProperties())
                property.SetColumnName(property.GetColumnName(objectIdentifier)?.Underscore());

            foreach (var key in entity.GetKeys())
                key.SetName(key.GetName()?.Underscore());

            foreach (var fk in entity.GetForeignKeys())
                fk.SetConstraintName(fk.GetConstraintName()?.Underscore());
        }

        // ----------------------------
        // Relationships
        // ----------------------------

        // Module → RoleGroups
        builder.Entity<RoleGroup>()
            .HasOne(rg => rg.Module)
            .WithMany(m => m.RoleGroups)
            .HasForeignKey(rg => rg.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);

        // RoleGroup → Roles
        builder.Entity<ApplicationRole>()
            .HasOne(r => r.RoleGroup)
            .WithMany(rg => rg.Roles)
            .HasForeignKey(r => r.RoleGroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // ClaimGroup → Claims
        builder.Entity<ApplicationClaim>()
            .HasOne(c => c.ClaimGroup)
            .WithMany(cg => cg.Claims)
            .HasForeignKey(c => c.ClaimGroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // ClaimGroupClaim (Many-to-Many)
        builder.Entity<ClaimGroupClaim>(entity =>
        {
            entity.HasKey(x => new { x.ClaimGroupId, x.ClaimId });

            entity.HasOne(x => x.ClaimGroup)
                  .WithMany()
                  .HasForeignKey(x => x.ClaimGroupId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Claim)
                  .WithMany()
                  .HasForeignKey(x => x.ClaimId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.ToTable("ClaimGroupClaims");
        });

        // Role ClaimGroup (many-to-many via RoleClaimGroup)
        builder.Entity<RoleClaimGroup>(entity =>
        {
            entity.ToTable("RoleClaimGroups");

            entity.HasKey(rcg => new { rcg.RoleId, rcg.ClaimGroupId });

            entity.HasOne(rcg => rcg.Role)
                .WithMany(r => r.RoleClaimGroups)
                .HasForeignKey(rcg => rcg.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(rcg => rcg.ClaimGroup)
                .WithMany(cg => cg.RoleClaimGroups)
                .HasForeignKey(rcg => rcg.ClaimGroupId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ----------------------------
        // RoleClaims
        // ----------------------------
        builder.Entity<RoleClaim>(entity =>
        {
            entity.ToTable("RoleClaims");
            entity.HasKey(x => new { x.RoleId, x.ClaimId });

            entity.Property(x => x.RoleId).IsRequired();
            entity.Property(x => x.ClaimId).IsRequired();

            entity.HasOne(x => x.Role)
                  .WithMany()
                  .HasForeignKey(x => x.RoleId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Claim)
                  .WithMany()
                  .HasForeignKey(x => x.ClaimId)
                  .OnDelete(DeleteBehavior.NoAction);
        });
    }

    // ----------------------------
    // Transaction helpers
    // ----------------------------
    public Task ExecuteTransactionalAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        var strategy = Database.CreateExecutionStrategy();
        return strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await Database
                .BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
            try
            {
                await action();
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }

    public Task<T> ExecuteTransactionalAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default)
    {
        var strategy = Database.CreateExecutionStrategy();
        return strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await Database
                .BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
            try
            {
                var result = await action();
                await transaction.CommitAsync(cancellationToken);
                return result;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }

    // ----------------------------
    // Domain events
    // ----------------------------
    public IReadOnlyList<IDomainEvent> GetAllUncommittedEvents() => new List<IDomainEvent>();

    public void MarkUncommittedDomainEventAsCommitted()
    {
        // intentionally left empty
    }
}
