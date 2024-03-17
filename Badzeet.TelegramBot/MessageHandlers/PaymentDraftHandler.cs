using Telegram.Bot;

namespace Badzeet.TelegramBot.MessageHandlers;

public class PaymentDraftHandler : IHandler
{
    public Task Handler(ITelegramBotClient botClient, ChatState state, string messageText, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}