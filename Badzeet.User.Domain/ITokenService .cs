using System;
using System.Threading.Tasks;

namespace Badzeet.User.Domain
{
    public interface ITokenService
    {
        Task<IssueTokenResponse> Issue(string username, string password);
        Task<bool> Validate(Guid userid, string token);
        Task<IssueTokenResponse> Refresh(Guid userId, string token);
    }
}
