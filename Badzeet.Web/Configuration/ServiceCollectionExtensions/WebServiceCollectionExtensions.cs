using Badzeet.Web.Features.Account;
using Badzeet.Web.Features.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Badzeet.Web.Configuration.ServiceCollectionExtensions;

public static class WebServiceCollectionExtensions
{
    public static void RegisterServiceDependencies(this IServiceCollection services)
    {
        services.AddTransient<Service>();
        services.AddTransient<Features.ScheduledPayments.Service>();
        services.AddTransient<BudgetNavigationService>();
    }
}