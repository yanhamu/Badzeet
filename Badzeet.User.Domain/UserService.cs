using System;
using System.Threading.Tasks;

namespace Badzeet.User.Domain
{
    public partial class UserService : IUserService
    {
        private readonly UserRepository userRepository;

        public UserService(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<UserLoginResponse> Check(string username, string password)
        {
            var userDto = await userRepository.GetUser(username);
            if (userDto is null)
                return UserLoginResponse.CreateFailed();

            return PasswordService.Verify(userDto.Password, password)
                ? (UserLoginResponse)UserLoginResponse.CreateSuccessful(userDto.Id)
                : UserLoginResponse.CreateFailed();
        }

        public async Task<bool> CheckAvailability(string username)
        {
            var user = await userRepository.GetUser(username);
            return user is null;
        }

        public async Task<Guid> RegisterUser(string username, string password)
        {
            var hashedPassword = PasswordService.GetHashedPassword(password);
            var userId = Guid.NewGuid();
            await userRepository.CreateUser(new UserDto() { Id = userId, Password = hashedPassword, Username = username });
            return userId;
        }
    }
}