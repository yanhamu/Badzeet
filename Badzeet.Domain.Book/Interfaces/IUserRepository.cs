using System;
using System.Threading.Tasks;

namespace Badzeet.Domain.Book.Interfaces
{
    public interface IUserRepository
    {
        public Task Create(Guid id);
    }
}
