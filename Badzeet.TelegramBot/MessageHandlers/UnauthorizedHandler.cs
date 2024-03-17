using Telegram.Bot;

namespace Badzeet.TelegramBot.MessageHandlers;

public class UnauthorizedHandler : IHandler
{
    public async Task Handler(ITelegramBotClient botClient, ChatState state, string messageText, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(state.ChatId, "I'm not talking to you. Use `auth [pass]` first", cancellationToken: cancellationToken);
    }
}