﻿using Badzeet.Budget.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Badzeet.Web.Configuration.ServiceCollectionExtensions;

public static class IntegrationHandlersServiceCollectionExtensions
{
    public static void RegisterIntegrationHandlers(this IServiceCollection services)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<PaymentsService>());
    }
}