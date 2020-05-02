using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Badzeet.Web.Configuration.ServiceCollectionExtensions
{
    public static class WebServiceCollectionExtensions
    {
        public static void RegisterServiceDependencies(this IServiceCollection services)
        {
            services.AddTransient<Features.Account.Service>();
            services.AddTransient<Features.ScheduledPayments.Service>();
        }
    }
}
