using Badzeet.Domain.Book.Interfaces;
using Badzeet.Domain.Budget;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.ScheduledPayments
{
    public class ScheduledPayments : ViewComponent
    {
        private readonly ScheduledPaymentsService service;
        private readonly ICategoryRepository categoryRepository;

        public ScheduledPayments(ScheduledPaymentsService service, ICategoryRepository categoryRepository)
        {
            this.service = service;
            this.categoryRepository = categoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid userId, long bookId)
        {
            var categories = await categoryRepository.GetCategories(bookId);
            var payments = await service.GetPayments(userId);
            var paymentsModel = payments.Select(x => new ScheduledPaymentViewModel()
            {
                Description = x.Description,
                ScheduledPaymentId = x.Id,
                Amount = x.Amount,
                CategoryId = x.CategoryId,
                CategoryName = categories.Single(c => c.Id == x.CategoryId).Name,
                OwnerId = x.OwnerId,
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