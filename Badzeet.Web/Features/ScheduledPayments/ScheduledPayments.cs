using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.ScheduledPayments
{
    public class ScheduledPayments : ViewComponent
    {
        private readonly IPaymentRepository repository;
        private readonly ICategoryRepository categoryRepository;

        public ScheduledPayments(IPaymentRepository repository, ICategoryRepository categoryRepository)
        {
            this.repository = repository;
            this.categoryRepository = categoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid userId, long accountId)
        {
            var categories = await categoryRepository.GetCategories(accountId);
            var scheduledPayments = await repository.GetPayments(new PaymentsFilter(accountId, userId, type: PaymentType.Scheduled));
            var paymentsModel = scheduledPayments.Select(x => new ScheduledPaymentViewModel()
            {
                Description = x.Description,
                ScheduledPaymentId = x.Id,
                Amount = x.Amount,
                CategoryId = x.CategoryId,
                CategoryName = categories.Single(c => c.Id == x.CategoryId).Name,
                OwnerId = x.UserId,
                Date = x.Date
            });

            return View(paymentsModel);
        }

        public class ScheduledPaymentViewModel
        {
            public long ScheduledPaymentId { get; set; }
            public DateTime Date { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }
            public long CategoryId { get; set; }
            public string CategoryName { get; set; }
            public Guid OwnerId { get; set; }
        }
    }
}