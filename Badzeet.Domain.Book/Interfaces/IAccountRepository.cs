using Badzeet.Domain.Book.Model;
using System.Threading.Tasks;

namespace Badzeet.Domain.Book.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> GetAccount(long accountId);
        Task<Account> CreateAccount(byte firstDayOfBudget);
    }
}
