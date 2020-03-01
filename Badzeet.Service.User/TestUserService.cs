using System;
using System.Threading.Tasks;

namespace Badzeet.Service.User
{
    public class TestUserService : IUserService
    {
        public Task<UserLoginResponse> Check(string username, string password)
        {
            if (username == "bob" && password == "bob")
                return Task.FromResult<UserLoginResponse>(UserLoginResponse.CreateSuccessful(1));

            return Task.FromResult<UserLoginResponse>(UserLoginResponse.CreateFailed());
        }

        public Task<bool> CheckAvailability(string username)
        {
            return Task.FromResult(username != "bob");
        }

        public Task RegisterUser(string username, string password)
        {
            if (username == "bob")
                throw new Exception();

            return Task.CompletedTask;
        }
    }
}
