using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.BackgroundServices
{
    public class NightlyTaskService : BackgroundService
    {
        private readonly ILogger<NightlyTaskService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public NightlyTaskService(ILogger<NightlyTaskService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("NightlyTaskService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.Now;
                    var nextRun = now.Date.AddDays(1);
                    var delay = nextRun - now;
                    await Task.Delay(delay, stoppingToken);
                    await RunNightlyTaskAsync(stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    // shutting down
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in NightlyTaskService.");
                }
            }
        }

        private async Task RunNightlyTaskAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

            await CleanupExpiredRefreshTokensAsync(db, cancellationToken);
        }

        private async Task CleanupExpiredRefreshTokensAsync(IApplicationDbContext db, CancellationToken ct)
        {
            // Keep revoked tokens for a 7-day buffer so we can still detect reuse attempts shortly after expiry.
            var cutoff = DateTime.UtcNow.AddDays(-7);

            var deleted = await db.RefreshTokens
                .Where(t => t.ExpiresAt < cutoff)
                .ExecuteDeleteAsync(ct);

            if (deleted > 0)
                _logger.LogInformation("Removed {Count} expired refresh tokens.", deleted);
        }
    }
}
