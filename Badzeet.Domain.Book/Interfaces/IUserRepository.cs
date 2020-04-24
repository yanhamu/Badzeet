using System;
using System.Threading.Tasks;

namespace Badzeet.Domain.Budget.Interfaces
{
    public interface IUserRepository
    {
        public Task Create(Guid id, string username);
    }
}
