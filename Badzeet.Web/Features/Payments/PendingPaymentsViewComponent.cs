using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace Badzeet.Web.Features.Payments;

public class PendingPaymentsViewComponent : ViewComponent
{
    private readonly ICategoryRepository categoryRepository;
    private readonly IPaymentRepository repository;

    public PendingPaymentsViewComponent(IPaymentRepository repository,
        ICategoryRepository categoryRepository)
    {
        this.repository = repository;
        this.categoryRepository = categoryRepository;
    }

    public async Task<IViewComponentResult> InvokeAsync(long accountId)
    {
        var pendingPayments = await repository.GetPayments(new PaymentsFilter(accountId, null, null, null, PaymentType.Pending));
        var categories = await categoryRepository.GetCategories(accountId);
        var cMap = categories.ToDictionary(k => k.Id, v => v.Name);
        var paymentsModel = pendingPayments.Select(x => new PendingPaymentViewModel
        {
            Description = x.Description,
            Id = x.Id,
            Amount = x.Amount,
            CategoryName = cMap[x.CategoryId],
            CategoryId = x.CategoryId,
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
        public decimal Total => PendingPayments.Sum(x => x.Amount);
    }

    public class PendingPaymentViewModel
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Owner { get; set; } = string.Empty;
        public Guid OwnerId { get; set; }
    }
}