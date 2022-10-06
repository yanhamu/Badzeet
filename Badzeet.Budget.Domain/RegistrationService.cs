using Badzeet.Budget.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Badzeet.Budget.Domain
{
    public class RegistrationService
    {
        private readonly IUserAccountRepository userAccountRepository;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IAccountRepository accountRepository;

        public RegistrationService(
            IUserAccountRepository userBookRepository,
            IAccountRepository bookRepository,
            IUserRepository userRepository,
            ICategoryRepository categoryRepository)
        {
            this.accountRepository = bookRepository;
            this.userAccountRepository = userBookRepository;
            this.userRepository = userRepository;
            this.categoryRepository = categoryRepository;
        }

        public async Task Register(Guid userId, string username)
        {
            await userRepository.Create(userId, username);

           
                var account = await accountRepository.CreateAccount(1);
                _ = await userAccountRepository.Create(userId, account.Id);
                await categoryRepository.Create(account.Id, "unspecified", 1000);
            
        }
    }
}