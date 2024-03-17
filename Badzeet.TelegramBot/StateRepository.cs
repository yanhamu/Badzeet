namespace Badzeet.TelegramBot;

public class StateRepository
{
    public ChatState Get(long chatId)
    {
        if (chatId == 5699106366)
        {
            return new ChatState()
            {
                ChatId = chatId,
                State = State.Initialized,
                UserId = Guid.Empty,
                AccountId = 1
            };
        }

        return new ChatState()
        {
            State = State.Unauthorized,
            UserId = null,
            ChatId = chatId,
            AccountId = null
        };
    }
}

public class ChatState
{
    public long ChatId { get; set; }
    public long? AccountId { get; set; }
    public Guid? UserId { get; set; }
    public State State { get; set; }
}

public enum State
{
    Unauthorized = 1,
    Initialized = 2,
    PaymentDraft = 3
}