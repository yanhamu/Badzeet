using Badzeet.Domain.Book.Model;
using System;
using System.Threading.Tasks;

namespace Badzeet.Domain.Book.Interfaces
{
    public interface IInvitationRepository
    {
        Task<Invitation> Get(Guid id);
        Task<Invitation> Create(Guid userId, long bookId);
    }
}
