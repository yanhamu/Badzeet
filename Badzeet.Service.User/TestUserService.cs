using System;
using System.Threading.Tasks;

namespace Badzeet.Service.User
{
    public class TestUserService : IUserService
    {
        public Task<UserLoginResponse> Check(string username, string password)
        {
            if (username == "bob" && password == "bob")
                return Task.FromResult<UserLoginResponse>(UserLoginResponse.CreateSuccessful(new Guid("5ECCAEE6-735A-4A2B-9176-526577D648AC")));

            return Task.FromResult<UserLoginResponse>(UserLoginResponse.CreateFailed());
        }

        public Task<bool> CheckAvailability(string username)
        {
            return Task.FromResult(username != "bob");
        }

        public Task<Guid> RegisterUser(string username, string password)
        {
            if (username == "bob")
                throw new Exception();

            return Task.FromResult(Guid.NewGuid());
        }
    }
}
