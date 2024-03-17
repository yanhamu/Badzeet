using Telegram.Bot;

namespace Badzeet.TelegramBot.MessageHandlers;

public interface IHandler
{
    Task Handler(ITelegramBotClient botClient, ChatState state, string messageText, CancellationToken cancellationToken);
}