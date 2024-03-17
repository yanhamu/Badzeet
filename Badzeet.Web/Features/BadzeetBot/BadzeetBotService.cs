using System;
using System.Threading;
using System.Threading.Tasks;
using Badzeet.TelegramBot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Badzeet.Web.Features.BadzeetBot;

public class BadzeetBotService : BackgroundService
{
    private readonly IServiceProvider serviceProvider;
    private readonly ILogger<BadzeetBotService> logger;

    public BadzeetBotService(IServiceProvider serviceProvider, ILogger<BadzeetBotService> logger)
    {
        this.serviceProvider = serviceProvider;
        this.logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var options = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
        };

        var botClient = serviceProvider.GetRequiredService<TelegramBotClient>();
        botClient.StartReceiving(updateHandler: HandleUpdates, pollingErrorHandler: HandleErrors, receiverOptions: options, cancellationToken: stoppingToken);
        return Task.CompletedTask;
    }

    private async Task HandleUpdates(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var botService = scope.ServiceProvider.GetRequiredService<BotService>();
        await botService.HandleUpdateAsync(botClient, update, cancellationToken);
    }

    private Task HandleErrors(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Telegram bot error");
        return Task.CompletedTask;
    }
}