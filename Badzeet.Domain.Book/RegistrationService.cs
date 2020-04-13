using Badzeet.Domain.Book.Interfaces;
using System;
using System.Threading.Tasks;

namespace Badzeet.Domain.Book
{
    public class RegistrationService
    {
        private readonly IUserBookRepository userBookRepository;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IInvitationRepository invitationRepository;
        private readonly IAccountRepository bookRepository;

        public RegistrationService(
            IUserBookRepository userBookRepository,
            IInvitationRepository invitationRepository,
            IAccountRepository bookRepository,
            IUserRepository userRepository,
            ICategoryRepository categoryRepository)
        {
            this.bookRepository = bookRepository;
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
                var book = await bookRepository.CreateAccount(1);
                _ = await userBookRepository.Create(userId, book.Id);
                await categoryRepository.Create(book.Id, "unspecified", 1000);
            }
        }
    }
}