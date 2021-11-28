using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.Payments
{
    public class PendingPaymentsViewComponent : ViewComponent
    {
        private readonly IPaymentRepository repository;
        private readonly ICategoryRepository categoryRepository;

        public PendingPaymentsViewComponent(IPaymentRepository repository, ICategoryRepository categoryRepository)
        {
            this.repository = repository;
            this.categoryRepository = categoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(long accountId)
        {
            var categories = await categoryRepository.GetCategories(accountId);
            var pendingPayments = await repository.GetPayments(new PaymentsFilter(accountId, null, null, null, PaymentType.Pending));
            var paymentsModel = pendingPayments.Select(x => new PendingPaymentViewModel()
            {
                Description = x.Description,
                Id = x.Id,
                Amount = x.Amount,
                CategoryId = x.CategoryId,
                CategoryName = categories.Single(c => c.Id == x.CategoryId).Name,
                OwnerId = x.UserId,
                Date = x.Date,
                Owner = x.User.Nickname
            });

            return View(new PendingPaymentsViewModel(paymentsModel));
        }

        public class PendingPaymentsViewModel
        {
            public PendingPaymentsViewModel(IEnumerable<PendingPaymentViewModel> pendingPayments)
            {
                PendingPayments = pendingPayments;
            }

            public IEnumerable<PendingPaymentViewModel> PendingPayments { get; }
            public decimal Total { get => PendingPayments.Sum(x => x.Amount); }
        }

        public class PendingPaymentViewModel
        {
            public long Id { get; set; }
            public DateTime Date { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }
            public long CategoryId { get; set; }
            public string CategoryName { get; set; }
            public string Owner { get; set; }
            public Guid OwnerId { get; set; }
        }
    }
}
