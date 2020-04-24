using Badzeet.Domain.Budget.Model;
using System;
using System.Threading.Tasks;

namespace Badzeet.Domain.Budget.Interfaces
{
    public interface IInvitationRepository
    {
        Task<Invitation> Get(Guid id);
        Task<Invitation> Create(Guid userId, long bookId);
    }
}
