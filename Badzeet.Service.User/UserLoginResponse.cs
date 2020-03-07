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
        public abstract Guid GetUserId();

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

        public override Guid GetUserId()
        {
            throw new InvalidOperationException("Can't access unauthorized user id");
        }
    }

    public class SuccessfulUserLoginResponse : UserLoginResponse
    {
        private readonly Guid userId;

        public SuccessfulUserLoginResponse(Guid userId) : base(true)
        {
            this.userId = userId;
        }

        public override Guid GetUserId()
        {
            return userId;
        }
    }
}
