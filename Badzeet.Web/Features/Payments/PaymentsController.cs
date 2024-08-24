using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Badzeet.Budget.Domain;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Badzeet.Web.Features.Payments;

[Authorize]
public class PaymentsController : Controller
{
    private readonly IAccountRepository accountRepository;
    private readonly BudgetService budgetService;
    private readonly ICategoryRepository categoryRepository;
    private readonly IPaymentRepository paymentRepository;
    private readonly PaymentsService paymentsService;
    private readonly IUserAccountRepository userAccountRepository;

    public PaymentsController(
        PaymentsService paymentsService,
        ICategoryRepository categoryRepository,
        IUserAccountRepository userAccountRepository,
        IPaymentRepository paymentRepository,
        BudgetService budgetService,
        IAccountRepository accountRepository)
    {
        this.paymentsService = paymentsService;
        this.categoryRepository = categoryRepository;
        this.paymentRepository = paymentRepository;
        this.userAccountRepository = userAccountRepository;
        this.budgetService = budgetService;
        this.accountRepository = accountRepository;
    }

    [HttpGet]
    public async Task<IActionResult> New(long accountId, Guid userId)
    {
        var categories = await categoryRepository.GetCategories(accountId);
        var users = await userAccountRepository.GetUsers(accountId);
        var model = new PaymentViewModel
        {
            Categories = categories.Select(x => new CategoryViewModel { Name = x.Name, Id = x.Id }).ToList(),
            Users = users
        };

        model.Payment.Date = DateTime.Now;
        model.Payment.UserId = userId;
        model.Payment.Type = PaymentType.Normal;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> New(long accountId, PaymentViewModel model)
    {
        await paymentsService.Add(new Payment(model.Payment.Date, model.Payment.Description, model.Payment.Amount, model.Payment.CategoryId, model.Payment.UserId, model.Payment.Type, accountId));

        return RedirectToAction("List");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(long id, long accountId)
    {
        var transaction = await paymentsService.GetPayment(id);
        var categories = await categoryRepository.GetCategories(accountId);
        var users = await userAccountRepository.GetUsers(accountId);
        var model = new PaymentViewModel
        {
            Categories = categories.Select(x => new CategoryViewModel { Id = x.Id, Name = x.Name }).ToList(),
            Payment = new PaymentModel(transaction!),
            Users = users
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(long accountId, PaymentViewModel model)
    {
        await paymentsService.Save(new Payment(
            model.Payment.Id,
            model.Payment.Date,
            model.Payment.Description,
            model.Payment.Amount,
            model.Payment.CategoryId,
            model.Payment.UserId,
            model.Payment.Type,
            accountId));

        return RedirectToAction("List");
    }

    [HttpGet]
    public async Task<IActionResult> Split(long id, long accountId)
    {
        var payment = await paymentRepository.Get(id);
        var categories = await categoryRepository.GetCategories(accountId);
        var users = await userAccountRepository.GetUsers(accountId);
        var model = new PaymentViewModel
        {
            Categories = categories.Select(x => new CategoryViewModel { Id = x.Id, Name = x.Name }).ToList(),
            Payment = new PaymentModel(payment!),
            Users = users
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Split(SplitModel model)
    {
        await paymentsService.Split(model.OldPaymentId, model.OldAmount, model.Description, model.NewAmount, model.CategoryId, model.OwnerId);
        return RedirectToAction("List");
    }

    [HttpPost]
    public async Task<IActionResult> Remove([FromForm(Name = "Payment.Id")] long id)
    {
        await paymentsService.Remove(id);
        return RedirectToAction("List");
    }

    [HttpGet]
    public async Task<IActionResult> List(long accountId, [FromQuery(Name = "cid")] long[] categoryIds, DateTime? from, DateTime? to)
    {
        var account = await accountRepository.GetAccount(accountId);
        var interval = GetInterval(account!, from, to);
        var payments = await paymentRepository.GetPayments(new PaymentsFilter(
            accountId,
            categoryIds ?? Array.Empty<long>(),
            null,
            interval.From,
            interval.To,
            PaymentType.Normal));

        var c = await categoryRepository.GetCategories(accountId);
        var users = await userAccountRepository.GetUsers(accountId);

        var model = new PaymentsViewModel
        {
            Categories = c.Select(x => new CategoryViewModel { Id = x.Id, Name = x.Name }).ToList(),
            Payments = payments.Select(x => new PaymentModel(x.Id, x.Date, x.Description, x.Amount, x.CategoryId, x.UserId, x.Type)),
            Users = users,
            DateInterval = interval,
            CategoryIds = categoryIds ?? Array.Empty<long>()
        };

        return View(model);
    }

    private DateInterval GetInterval(Badzeet.Budget.Domain.Model.Account account, DateTime? from, DateTime? to)
    {
        if (from.HasValue == false || to.HasValue == false)
            return budgetService.GetBudgetInterval(account.FirstDayOfTheBudget, DateTime.UtcNow);

        return new DateInterval(from.Value, to.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Transform(long id)
    {
        await paymentsService.Transform(id);
        return Redirect(Request.Headers["Referer"].ToString());
    }

    public class CategoryViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class PaymentViewModel
    {
        public List<CategoryViewModel> Categories { get; set; } = new();
        public PaymentModel Payment { get; set; } = new();
        public IEnumerable<UserAccount> Users { get; set; } = new List<UserAccount>();
    }

    public class PaymentModel
    {
        public PaymentModel()
        {
        }

        public PaymentModel(long id, DateTime date, string description, decimal amount, long categoryId, Guid userId, PaymentType type)
        {
            Id = id;
            Date = date;
            Description = description;
            Amount = amount;
            CategoryId = categoryId;
            UserId = userId;
            Type = type;
        }

        public PaymentModel(Payment payment)
        {
            Id = payment.Id;
            Date = payment.Date;
            Description = payment.Description;
            Amount = payment.Amount;
            CategoryId = payment.CategoryId;
            UserId = payment.UserId;
            Type = payment.Type;
        }

        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public long CategoryId { get; set; }
        public Guid UserId { get; set; }
        public PaymentType Type { get; set; }
    }

    public class PaymentsViewModel
    {
        public long[] CategoryIds = Array.Empty<long>();
        public List<CategoryViewModel> Categories { get; set; } = new();
        public IEnumerable<PaymentModel> Payments { get; set; } = new List<PaymentModel>();
        public IEnumerable<UserAccount> Users { get; set; } = new List<UserAccount>();
        public DateInterval DateInterval { get; set; }
    }

    public class SplitModel
    {
        public long OldPaymentId { get; set; }
        public decimal OldAmount { get; set; }
        public decimal NewAmount { get; set; }
        public long CategoryId { get; set; }
        public Guid OwnerId { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}