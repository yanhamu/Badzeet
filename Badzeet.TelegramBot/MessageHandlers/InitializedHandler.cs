using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Badzeet.TelegramBot.MessageHandlers;

public class InitializedHandler : IHandler
{
    public async Task Handler(ITelegramBotClient botClient, ChatState state, string messageText, CancellationToken cancellationToken)
    {
        var (description, amount) = Parse(messageText);
        if (description == string.Empty || amount == default)
        {
            await botClient.SendTextMessageAsync(
                chatId: state.ChatId,
                text: "Sorry, didn't get that. Send me [amount] [description] so I can process it.",
                cancellationToken: cancellationToken);
            return;
        }

        var categories = GetCategories();

        var layout = categories.Select((item, index) => (item, index))
            .GroupBy(x => x.index / 3)
            .Select(group => group.Select(x => new KeyboardButton(x.item)));

        var markup = new ReplyKeyboardMarkup(layout);
        var m = $"What category for {description} {amount}";
        await botClient.SendTextMessageAsync(chatId: state.ChatId, text: m, replyMarkup: markup,
            cancellationToken: cancellationToken);
    }
    private (string description, decimal amount) Parse(string messageText)
    {
        var a = default(decimal);
        var d = new StringBuilder();

        for (int i = 0; i < messageText.Length; i++)
        {
            if (messageText[i] == '-' && i + 1 < messageText.Length && char.IsNumber(messageText[i + 1]))
            {
                a *= -1;
            }
            else if (char.IsNumber(messageText[i]))
            {
                a *= 10;
                a += messageText[i] - '0';
            }
            else
            {
                d.Append(messageText[i]);
            }
        }

        return (d.ToString().Trim(), a);
    }

    private static string[] GetCategories()
    {
        var categories = new[]
        {
            "food", "Omas lunch", "restaurants", "household", "drugstore", "clothes", "entertainment", "others",
            "HR/SK", "sport", "traveling", "gifts", "ftb"
        };
        return categories;
    }
}