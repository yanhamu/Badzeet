using Badzeet.TelegramBot;
using Badzeet.TelegramBot.MessageHandlers;
using Badzeet.Web.Features.BadzeetBot;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace Badzeet.Web.Configuration.ServiceCollectionExtensions;

public static class BadzeetBotServiceCollectionExtensions
{
    public static void RegisterBadzeetBot(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<TelegramBotClient>(x=> new TelegramBotClient(configuration["BotToken"]!));
        services.AddHostedService<BadzeetBotService>();
        services.AddScoped<BotService>();

        services.AddTransient<MessageHandlerFactory>();

        services.AddTransient<StateRepository>();
    }
}