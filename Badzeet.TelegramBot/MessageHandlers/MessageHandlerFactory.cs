namespace Badzeet.TelegramBot.MessageHandlers;

public class MessageHandlerFactory
{
    public IHandler Create(State stateState)
    {
        return stateState switch
        {
            State.Unauthorized => new UnauthorizedHandler(),
            State.Initialized => new InitializedHandler(),
            State.PaymentDraft => new PaymentDraftHandler(),
            _ => throw new ArgumentOutOfRangeException(nameof(stateState), stateState, null)
        };
    }
}