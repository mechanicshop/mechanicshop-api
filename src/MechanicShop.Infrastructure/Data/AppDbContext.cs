using System.Linq.Expressions;

using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domain.Common;
using MechanicShop.Domain.Customers;
using MechanicShop.Domain.Customers.Vehicles;
using MechanicShop.Domain.Employees;
using MechanicShop.Domain.Identity;
using MechanicShop.Domain.RepairTasks;
using MechanicShop.Domain.RepairTasks.Parts;
using MechanicShop.Domain.Workorders;
using MechanicShop.Domain.Workorders.Billing;
using MechanicShop.Infrastructure.Identity;

using MediatR;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MechanicShop.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options, IMediator mediator)
    : IdentityDbContext<AppUser>(options), IAppDbContext
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Part> Parts => Set<Part>();
    public DbSet<RepairTask> RepairTasks => Set<RepairTask>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        await DispatchDomainEventsAsync(ct);
        return await base.SaveChangesAsync(ct);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var propertyAccess = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
                var notExpression = Expression.Not(propertyAccess);
                var lambda = Expression.Lambda(notExpression, parameter);
                builder.Entity(entityType.ClrType).HasQueryFilter(lambda);

                builder.Entity(entityType.ClrType)
                    .HasIndex(nameof(ISoftDelete.IsDeleted))
                    .HasFilter($"\"{nameof(ISoftDelete.IsDeleted)}\" = false");
            }
        }
    }

    private async Task DispatchDomainEventsAsync(CancellationToken ct)
    {
        var domainEntities = ChangeTracker.Entries()
            .Where(e => e.Entity is Entity baseEntity && baseEntity.DomainEvents.Count != 0)
            .Select(e => (Entity)e.Entity)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, ct);
        }

        foreach (var entity in domainEntities)
        {
            entity.ClearDomainEvents();
        }
    }
}
