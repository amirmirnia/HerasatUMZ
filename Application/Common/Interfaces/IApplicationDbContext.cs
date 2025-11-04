using Microsoft.EntityFrameworkCore;
using Domain.Entities.Users;
using Domain.Entities.Visitors;


namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{

    DbSet<User> Users { get; }
    DbSet<Visitor> Visitors { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}