using MechanicShop.Api.Domain.Customers;
using MechanicShop.Api.Domain.Customers.Vehicles;
using MechanicShop.Api.Domain.Employees;
using MechanicShop.Api.Domain.Identity;
using MechanicShop.Api.Domain.RepairTasks;
using MechanicShop.Api.Domain.RepairTasks.Parts;
using MechanicShop.Api.Domain.Workorders;
using MechanicShop.Api.Domain.Workorders.Billing;

using Microsoft.EntityFrameworkCore;

namespace MechanicShop.Api.Common.Interfaces;

public interface IAppDbContext
{
    public DbSet<Customer> Customers { get; }
    public DbSet<Part> Parts { get; }
    public DbSet<RepairTask> RepairTasks { get; }
    public DbSet<Vehicle> Vehicles { get; }
    public DbSet<WorkOrder> WorkOrders { get; }
    public DbSet<Employee> Employees { get; }
    public DbSet<Invoice> Invoices { get; }
    public DbSet<RefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken ct);
}
