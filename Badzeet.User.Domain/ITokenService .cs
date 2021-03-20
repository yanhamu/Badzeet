using System;
using System.Threading.Tasks;

namespace Badzeet.User.Domain
{
    public interface ITokenService
    {
        Task<IssueTokenResponse> Issue(string username, string password);
        Task<Guid?> FindTokenId(Guid userid, string token);
    }
}
