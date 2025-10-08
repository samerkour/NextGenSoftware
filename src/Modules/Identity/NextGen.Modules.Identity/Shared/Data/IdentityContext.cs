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
    // New DbSets for Modules, RoleGroups, ClaimGroups, Claims
    // ----------------------------
    public DbSet<NextGen.Modules.Identity.Shared.Models.ApplicationModule> Modules { get; set; } = default!;
    public DbSet<RoleGroup> RoleGroups { get; set; } = default!;
    public DbSet<ClaimGroup> ClaimGroups { get; set; } = default!;
    public DbSet<ApplicationClaim> Claims { get; set; } = default!;

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
            {
                property.SetColumnName(property.GetColumnName(objectIdentifier)?.Underscore());
            }

            foreach (var key in entity.GetKeys())
            {
                key.SetName(key.GetName()?.Underscore());
            }

            foreach (var fk in entity.GetForeignKeys())
            {
                fk.SetConstraintName(fk.GetConstraintName()?.Underscore());
            }
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
    // Domain events (no-op for now)
    // ----------------------------
    public IReadOnlyList<IDomainEvent> GetAllUncommittedEvents() => new List<IDomainEvent>();

    public void MarkUncommittedDomainEventAsCommitted()
    {
        // Method intentionally left empty.
    }
}
