using System;
using System.Threading.Tasks;
using Badzeet.Budget.Domain.Interfaces;

namespace Badzeet.Budget.Domain;

public class RegistrationService(
    IUserAccountRepository userBookRepository,
    IAccountRepository bookRepository,
    IUserRepository userRepository,
    ICategoryRepository categoryRepository)
{
    public async Task Register(Guid userId, string username)
    {
        await userRepository.Create(userId, username);

        var account = await bookRepository.CreateAccount(1);
        _ = await userBookRepository.Create(userId, account.Id);
        await categoryRepository.Create(account.Id, "unspecified", 1000);
    }
}