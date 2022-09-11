using Badzeet.Budget.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Badzeet.Budget.Domain
{
    public class RegistrationService
    {
        private readonly IUserAccountRepository userBookRepository;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IInvitationRepository invitationRepository;
        private readonly IAccountRepository accountRepository;

        public RegistrationService(
            IUserAccountRepository userBookRepository,
            IInvitationRepository invitationRepository,
            IAccountRepository bookRepository,
            IUserRepository userRepository,
            ICategoryRepository categoryRepository)
        {
            this.accountRepository = bookRepository;
            this.invitationRepository = invitationRepository;
            this.userBookRepository = userBookRepository;
            this.userRepository = userRepository;
            this.categoryRepository = categoryRepository;
        }

        public async Task Register(Guid userId, string username, Guid? invitationId)
        {
            await userRepository.Create(userId, username);

            if (invitationId.HasValue)
            {
                var invitation = await invitationRepository.Get(invitationId.Value);
                if (invitation.UsedAt.HasValue)
                    throw new InvalidOperationException("invitation is already used");
                invitation.UsedAt = DateTime.UtcNow;
                await userBookRepository.Create(userId, invitation.AccountId);
            }
            else
            {
                var book = await accountRepository.CreateAccount(1);
                _ = await userBookRepository.Create(userId, book.Id);
                await categoryRepository.Create(Guid.NewGuid(), book.Id, "unspecified", 1000);
            }
        }
    }
}