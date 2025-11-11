using Microsoft.EntityFrameworkCore;
using Domain.Entities.Users;
using Domain.Entities.Visitors;
using Domain.Entities.Log;
using Domain.Entities.Vehicles;


namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{

    DbSet<User> Users { get; }
    DbSet<Visitor> Visitors { get; }
    DbSet<Vehicle> Vehicles { get; }

    DbSet<VisitLog> visitLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}