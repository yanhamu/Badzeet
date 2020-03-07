using System;
using System.Threading.Tasks;

namespace Badzeet.Service.User
{
    public interface IUserService
    {
        Task<UserLoginResponse> Check(string username, string password);
        Task<bool> CheckAvailability(string username);
        Task<Guid> RegisterUser(string username, string password);
    }
}