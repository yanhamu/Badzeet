using System;

namespace Badzeet.Service.User
{
    public abstract class UserLoginResponse
    {
        public UserLoginResponse(bool isSuccessful)
        {
            this.IsSuccessful = isSuccessful;
        }
        public bool IsSuccessful { get; }
        public abstract long GetUserId();

        public static FailedUserLoginResponse CreateFailed()
        {
            return new FailedUserLoginResponse();
        }

        public static SuccessfulUserLoginResponse CreateSuccessful(long userId)
        {
            return new SuccessfulUserLoginResponse(userId);
        }
    }

    public class FailedUserLoginResponse : UserLoginResponse
    {
        public FailedUserLoginResponse() : base(false) { }

        public override long GetUserId()
        {
            throw new InvalidOperationException("Can't access unauthorized user id");
        }
    }

    public class SuccessfulUserLoginResponse : UserLoginResponse
    {
        private readonly long userId;

        public SuccessfulUserLoginResponse(long userId) : base(true)
        {
            this.userId = userId;
        }

        public override long GetUserId()
        {
            return userId;
        }
    }
}
