using Badzeet.Budget.Domain;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.Payments
{
    [Authorize]
    public class PaymentsController : Controller
    {
        private readonly PaymentsService paymentsService;
        private readonly ICategoryRepository categoryRepository;
        private readonly IPaymentRepository paymentRepository;
        private readonly IUserAccountRepository userAccountRepository;
        private readonly BudgetService budgetService;

        public PaymentsController(
            PaymentsService paymentsService,
            ICategoryRepository categoryRepository,
            IUserAccountRepository userAccountRepository,
            IPaymentRepository paymentRepository,
            BudgetService budgetService)
        {
            this.paymentsService = paymentsService;
            this.categoryRepository = categoryRepository;
            this.paymentRepository = paymentRepository;
            this.userAccountRepository = userAccountRepository;
            this.budgetService = budgetService;
        }

        [HttpGet]
        public async Task<IActionResult> New(long accountId, Guid userId)
        {
            var categories = await categoryRepository.GetCategories(accountId);
            var users = await userAccountRepository.GetUsers(accountId);
            var model = new PaymentViewModel()
            {
                Categories = categories.Select(x => new CategoryViewModel() { Id = x.Id, Name = x.Name }).ToList(),
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
            await paymentsService.Add(new Payment(default, model.Payment.Date, model.Payment.Description, model.Payment.Amount, model.Payment.CategoryId, model.Payment.UserId, model.Payment.Type, accountId));

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id, long accountId)
        {
            var transaction = await paymentsService.GetPayment(id);
            var categories = await categoryRepository.GetCategories(accountId);
            var users = await userAccountRepository.GetUsers(accountId);
            var model = new PaymentViewModel()
            {
                Categories = categories.Select(x => new CategoryViewModel() { Id = x.Id, Name = x.Name }).ToList(),
                Payment = new PaymentModel(transaction),
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
            var transaction = await paymentsService.GetPayment(id);
            var categories = await categoryRepository.GetCategories(accountId);
            var users = await userAccountRepository.GetUsers(accountId);
            var model = new PaymentViewModel()
            {
                Categories = categories.Select(x => new CategoryViewModel() { Id = x.Id, Name = x.Name }).ToList(),
                Payment = new PaymentModel(transaction),
                Users = users
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Split(SplitModel model)
        {
            var payment = await paymentsService.GetPayment(model.OldPaymentId);
            payment.Amount = model.OldAmount;
            var newPayment = new Payment(default, payment.Date, model.Description, model.NewAmount, model.CategoryId, model.OwnerId, PaymentType.Normal, payment.AccountId);
            await paymentsService.Add(newPayment);
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Remove([FromForm(Name = "Payment.Id")]long id)
        {
            await paymentsService.Remove(id);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List(long accountId, int budgetId, [FromQuery(Name = "cid")] long[] categoryIds)
        {
            var interval = await budgetService.GetMonthlyBudgetById(budgetId);

            var payments = await paymentRepository.GetPayments(new PaymentsFilter(accountId, categoryId: categoryIds, interval: interval, type: PaymentType.Normal));
            var categories = await categoryRepository.GetCategories(accountId);
            var users = await userAccountRepository.GetUsers(accountId);

            var model = new PaymentsViewModel()
            {
                Categories = categories.Select(x => new CategoryViewModel() { Id = x.Id, Name = x.Name }).ToList(),
                Payments = payments.Select(x => new PaymentModel(x.Id, x.Date, x.Description, x.Amount, x.CategoryId, x.UserId, x.Type)),
                Users = users
            };

            return View(model);
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
            public string Name { get; set; }
        }

        public class PaymentViewModel
        {
            public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
            public PaymentModel Payment { get; set; } = new PaymentModel();
            public IEnumerable<UserAccount> Users { get; set; } = new List<UserAccount>();
        }

        public class PaymentModel
        {
            public PaymentModel() { }

            public PaymentModel(long id, DateTime date, string description, decimal amount, long categoryId, Guid userId, PaymentType type)
            {
                Id = id;
                Date = date;
                Description = description;
                Amount = amount;
                this.CategoryId = categoryId;
                this.UserId = userId;
                this.Type = type;
            }

            public PaymentModel(Payment payment)
            {
                this.Id = payment.Id;
                this.Date = payment.Date;
                this.Description = payment.Description;
                this.Amount = payment.Amount;
                this.CategoryId = payment.CategoryId;
                this.UserId = payment.UserId;
                this.Type = payment.Type;
            }

            public long Id { get; set; }
            public DateTime Date { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }
            public long CategoryId { get; set; }
            public Guid UserId { get; set; }
            public PaymentType Type { get; set; }
        }

        public class PaymentsViewModel
        {
            public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
            public IEnumerable<PaymentModel> Payments { get; set; } = new List<PaymentModel>();
            public IEnumerable<UserAccount> Users { get; set; } = new List<UserAccount>();
        }

        public class SplitModel
        {
            public long OldPaymentId { get; set; }
            public decimal OldAmount { get; set; }
            public decimal NewAmount { get; set; }
            public long CategoryId { get; set; }
            public Guid OwnerId { get; set; }
            public string Description { get; set; }
        }
    }
}