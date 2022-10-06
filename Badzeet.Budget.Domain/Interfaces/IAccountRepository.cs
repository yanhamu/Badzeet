using Badzeet.Budget.Domain.Model;
using System;
using System.Threading.Tasks;

namespace Badzeet.Budget.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetAccount(long accountId);
        Task<Account> CreateAccount(byte firstDayOfBudget);
    }
}
