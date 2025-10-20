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

public class IdentityContext : IdentityDbContext<ApplicationUser, Role, Guid,
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
    public DbSet<RoleGroupRole> RoleGroupRoles { get; set; } = default!;

    public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

    // ----------------------------
    // Model Configuration
    // ----------------------------
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //// Remove AspNetRoleClaims
        //var roleClaimEntity = builder.Model.FindEntityType(typeof(IdentityRoleClaim<Guid>));
        //if (roleClaimEntity != null)
        //{
        //    builder.Model.RemoveEntityType(roleClaimEntity);
        //}

        //builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        //// ----------------------------
        //// Naming convention: underscore_case
        //// ----------------------------
        //foreach (var entity in builder.Model.GetEntityTypes())
        //{
        //    entity.SetTableName(entity.GetTableName()?.Underscore());

        //    var objectIdentifier =
        //        StoreObjectIdentifier.Table(entity.GetTableName()?.Underscore()!, entity.GetSchema());

        //    foreach (var property in entity.GetProperties())
        //        property.SetColumnName(property.GetColumnName(objectIdentifier)?.Underscore());

        //    foreach (var key in entity.GetKeys())
        //        key.SetName(key.GetName()?.Underscore());

        //    foreach (var fk in entity.GetForeignKeys())
        //        fk.SetConstraintName(fk.GetConstraintName()?.Underscore());
        //}

        // ----------------------------
        // Relationships
        // ----------------------------

        // RefreshToken mapping
        builder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(rt => rt.Id);
            entity.Property(rt => rt.Token).IsRequired();
            entity.HasOne(rt => rt.ApplicationUser)
                  .WithMany(u => u.RefreshTokens)
                  .HasForeignKey(rt => rt.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.ToTable("RefreshTokens");
        });

        builder.Entity<ApplicationUserRole>(entity =>
        {
            entity.ToTable("UserRoles");

            entity.HasKey(ur => new { ur.UserId, ur.RoleId });

            entity.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Module → RoleGroups
        builder.Entity<RoleGroup>()
            .HasOne(rg => rg.Module)
            .WithMany(m => m.RoleGroups)
            .HasForeignKey(rg => rg.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);

        // ClaimGroupClaim (Many-to-Many)
        builder.Entity<ClaimGroupClaim>(entity =>
        {
            entity.ToTable("ClaimGroupClaims");
            entity.HasKey(x => new { x.ClaimGroupId, x.ClaimId });

            entity.HasOne(x => x.ClaimGroup)
                .WithMany(x => x.ClaimGroupClaims)
                .HasForeignKey(x => x.ClaimGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Claim)
                .WithMany(x => x.ClaimGroupClaims)
                .HasForeignKey(x => x.ClaimId)
                .OnDelete(DeleteBehavior.Restrict);
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

            entity.HasOne(x => x.Role)
                .WithMany(r => r.RoleClaims)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Claim)
                .WithMany(c => c.RoleClaims)
                .HasForeignKey(x => x.ClaimId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        // RoleGroupRoles (Many-to-Many between RoleGroup and Role)
        builder.Entity<RoleGroupRole>(entity =>
        {
            entity.ToTable("RoleGroupRoles");

            entity.HasKey(x => new { x.RoleGroupId, x.RoleId });

            entity.HasOne(x => x.RoleGroup)
                .WithMany(rg => rg.RoleGroupRoles)
                .HasForeignKey(x => x.RoleGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Role)
                .WithMany(r => r.RoleGroupRoles)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
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
