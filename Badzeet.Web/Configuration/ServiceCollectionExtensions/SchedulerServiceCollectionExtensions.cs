using Badzeet.Scheduler.DataAccess;
using Badzeet.Scheduler.Domain;
using Badzeet.Scheduler.Domain.Interfaces;
using Badzeet.Scheduler.Domain.Processors;
using Badzeet.Web.Features.Scheduler;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Badzeet.Web.Configuration.ServiceCollectionExtensions;

public static class SchedulerServiceCollectionExtensions
{
    public static void RegisterSchedulerDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<PaymentsSchedulerHostedService>();
        services.AddTransient<PaymentScheduler>();
        services.AddDbContext<SchedulerDbContext>(options => { options.UseSqlite(configuration.GetConnectionString("badzeetDb")); });

        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<ILogRepository, LogRepository>();
        services.AddScoped<IProcessor, MonthlyPaymentProcessor>();
    }
}