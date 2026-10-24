using System.Security.Cryptography;
using System.Text;
using Application.Common.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class MeetingRequestRegistrationGuard(ApplicationDbContext context)
    : IMeetingRequestRegistrationGuard
{
    public async Task<T> ExecuteAsync<T>(
        string hostNationalCode,
        Func<Task<T>> register,
        CancellationToken cancellationToken)
    {
        // A transaction-owned SQL Server lock also protects against requests on other instances.
        var resource = "MeetingRequest:" + Convert.ToHexString(
            SHA256.HashData(Encoding.UTF8.GetBytes(hostNationalCode)));

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        await context.Database.ExecuteSqlInterpolatedAsync($"""
            DECLARE @result int;
            EXEC @result = sys.sp_getapplock
                @Resource = {resource},
                @LockMode = 'Exclusive',
                @LockOwner = 'Transaction',
                @LockTimeout = 15000;
            IF @result < 0
                THROW 51000, 'Unable to acquire meeting request lock.', 1;
            """, cancellationToken);

        var id = await register();
        await transaction.CommitAsync(cancellationToken);
        return id;
    }
}
