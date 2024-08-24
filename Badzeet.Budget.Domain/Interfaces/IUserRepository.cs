using System;
using System.Threading.Tasks;

namespace Badzeet.Budget.Domain.Interfaces;

public interface IUserRepository
{
    public Task Create(Guid id, string username);
}