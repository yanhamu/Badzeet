using Badzeet.Domain.Budget.Model;
using System.Threading.Tasks;

namespace Badzeet.Domain.Budget.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> GetAccount(long accountId);
        Task<Account> CreateAccount(byte firstDayOfBudget);
    }
}
