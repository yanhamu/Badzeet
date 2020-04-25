using Badzeet.Scheduler.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.Scheduler
{
    public class PaymentsSchedulerHostedService : IHostedService, IDisposable
    {
        private Timer timer;
        private readonly IServiceProvider serviceProvider;

        public PaymentsSchedulerHostedService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using var scope = serviceProvider.CreateScope();
            var scheduler = scope.ServiceProvider.GetRequiredService<PaymentScheduler>();
            scheduler.Run().Wait();
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}