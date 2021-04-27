using System;

namespace Badzeet.User.Domain
{
    public abstract class UserLoginResponse
    {
        public UserLoginResponse(bool isSuccessful)
        {
            this.IsSuccessful = isSuccessful;
        }
        public bool IsSuccessful { get; }
        public abstract Guid UserId { get; }

        public static FailedUserLoginResponse CreateFailed()
        {
            return new FailedUserLoginResponse();
        }

        public static SuccessfulUserLoginResponse CreateSuccessful(Guid userId)
        {
            return new SuccessfulUserLoginResponse(userId);
        }
    }

    public class FailedUserLoginResponse : UserLoginResponse
    {
        public FailedUserLoginResponse() : base(false) { }

        public override Guid UserId => throw new InvalidOperationException("Can't access unauthorized user id");
    }

    public class SuccessfulUserLoginResponse : UserLoginResponse
    {
        public SuccessfulUserLoginResponse(Guid userId) : base(true)
        {
            this.UserId = userId;
        }

        public override Guid UserId { get; }
    }
}