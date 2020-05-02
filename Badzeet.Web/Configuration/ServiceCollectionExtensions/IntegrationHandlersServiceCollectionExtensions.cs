using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Badzeet.Web.Configuration.ServiceCollectionExtensions
{
    public static class IntegrationHandlersServiceCollectionExtensions
    {
        public static void RegisterIntegrationHandlers(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Budget.Domain.PaymentsService));
        }
    }
}