using Badzeet.TelegramBot.MessageHandlers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Badzeet.TelegramBot;

public class BotService
{
    private readonly StateRepository stateRepository;
    private readonly MessageHandlerFactory messageHandlerFactory;

    public BotService(StateRepository stateRepository, MessageHandlerFactory messageHandlerFactory)
    {
        this.stateRepository = stateRepository;
        this.messageHandlerFactory = messageHandlerFactory;
    }
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message)
            return;
        if (message.Text is not { } messageText)
            return;

        var state = stateRepository.Get(message.Chat.Id);
        await botClient.SendTextMessageAsync(chatId: state.ChatId, text: $"chatId:{state.ChatId}", cancellationToken:cancellationToken);
        
        var handler = messageHandlerFactory.Create(state.State);
        await handler.Handler(botClient, state, messageText, cancellationToken);

    }
}