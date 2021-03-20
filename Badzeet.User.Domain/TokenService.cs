using System;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.User.Domain
{
    public class TokenService : ITokenService
    {
        private readonly UserRepository userRepository;
        private readonly TokenRepository tokenRepository;
        private const int tokenLength = 256;
        private readonly TimeSpan validityPeriod = TimeSpan.FromMinutes(30);

        public TokenService(UserRepository userRepository, TokenRepository tokenRepository)
        {
            this.userRepository = userRepository;
            this.tokenRepository = tokenRepository;
        }
        public async Task<IssueTokenResponse> Issue(string username, string password)
        {
            var user = await userRepository.GetUser(username);
            if (user == null)
                return new IssueTokenResponse();

            var doesMatch = PasswordService.Verify(user.Password, password);
            if (doesMatch == false)
                return new IssueTokenResponse();

            var token = PasswordService.GenerateToken(tokenLength);
            var expiration = DateTime.UtcNow.Add(validityPeriod);
            await tokenRepository.Save(user.Id, token, expiration);
            return IssueTokenResponse.Create(expiration, token, user.Id, user.Username);
        }

        public async Task<Guid?> FindTokenId(Guid userid, string token)
        {
            var tokens = await tokenRepository.Get(userid);
            var decoded = Convert.FromBase64String(token);
            var same = tokens.SingleOrDefault(x => x.IsSame(decoded));
            if (same == default)
                return null;

            return same.Id;
        }
    }

    public class IssueTokenResponse
    {
        public string Username { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsSuccess { get => Token != null; }

        internal static IssueTokenResponse Create(DateTime expire, byte[] token, Guid userId, string username)
        {
            return new IssueTokenResponse()
            {
                Expires = expire,
                Token = Convert.ToBase64String(token),
                UserId = userId,
                Username = username
            };
        }
    }
}