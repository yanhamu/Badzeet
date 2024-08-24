using System.Threading.Tasks;
using Badzeet.Budget.Domain.Model;

namespace Badzeet.Budget.Domain.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetAccount(long accountId);
    Task<Account> CreateAccount(byte firstDayOfBudget);
}