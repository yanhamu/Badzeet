using System;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.User.Domain
{
    public class TokenService : ITokenService
    {
        private readonly UserRepository userRepository;
        private readonly TokenRepository tokenRepository;
        private const int tokenLength = 128;
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
            DateTime expiration = GetExpiration();
            await tokenRepository.Save(user.Id, token, expiration);
            return IssueTokenResponse.Create(expiration, token, user.Id);
        }

        private DateTime GetExpiration()
        {
            return DateTime.UtcNow.Add(validityPeriod);
        }

        public async Task<bool> Validate(Guid userid, string token)
        {
            var t = await FindToken(userid, token);
            return t != default;
        }

        public async Task<IssueTokenResponse> Refresh(Guid userId, string token)
        {
            var staleToken = await FindToken(userId, token);
            if (staleToken == default)
            {
                return new IssueTokenResponse();
            }
            var newToken = PasswordService.GenerateToken(tokenLength);
            var expiration = GetExpiration();
            await tokenRepository.Save(userId, newToken, expiration);
            await tokenRepository.Remove(staleToken.Id);
            return IssueTokenResponse.Create(expiration, newToken, userId);
        }

        private async Task<TokenDto> FindToken(Guid userid, string token)
        {
            var tokens = await tokenRepository.Get(userid);
            var decoded = Convert.FromBase64String(token);
            var same = tokens.SingleOrDefault(x => x.IsSame(decoded));
            if (same == default)
                return null;

            return same;
        }
    }

    public class IssueTokenResponse
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsSuccess { get => Token != null; }

        internal static IssueTokenResponse Create(DateTime expire, byte[] token, Guid userId)
        {
            return new IssueTokenResponse()
            {
                Expires = expire,
                Token = Convert.ToBase64String(token),
                UserId = userId,
            };
        }
    }
}