using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                    //stop services
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in NightlyTaskService.");
                }
            }
        }

        private async Task RunNightlyTaskAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                //await mediator.Send(new CheckOutRengeReservtionCommand(), cancellationToken);
            }
            // شبیه‌سازی کار زمان‌بر
            await Task.Delay(2000); 
        }
    }
}
