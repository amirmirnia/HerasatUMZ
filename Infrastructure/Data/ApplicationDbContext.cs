using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;

using Domain.Entities.Users;
using Infrastructure.Data.Configurations;
using Domain.Entities.Visitors;
using Domain.Entities.Log;
using Domain.Entities.Vehicles;


namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly ICurrentUserService _currentUserService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<VisitLog> visitLogs => Set<VisitLog>();

    public DbSet<Visitor> Visitors => Set<Visitor>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);









        modelBuilder.ApplyConfiguration(new VisitorConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities();
        return await base.SaveChangesAsync(cancellationToken);
    }


    private void UpdateAuditableEntities()
    {
        var entries = ChangeTracker.Entries<Domain.Common.BaseEntity>();
        var currentUser = _currentUserService.UserId ?? "System";
        var now = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = now;
                    entry.Entity.CreatedBy = currentUser;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedDate = now;
                    entry.Entity.UpdatedBy = currentUser;
                    break;
            }
        }
    }
}