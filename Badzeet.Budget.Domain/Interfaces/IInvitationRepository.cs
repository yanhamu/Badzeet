using Badzeet.Budget.Domain.Model;
using System;
using System.Threading.Tasks;

namespace Badzeet.Budget.Domain.Interfaces
{
    public interface IInvitationRepository
    {
        Task<Invitation> Get(Guid id);
        Task<Invitation> Create(Guid userId, long bookId);
    }
}
