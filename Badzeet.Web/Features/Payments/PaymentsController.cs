using Badzeet.Domain.Book;
using Badzeet.Domain.Book.Interfaces;
using Badzeet.Domain.Book.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.Payments
{
    public class PaymentsController : Controller
    {
        private readonly PaymentsService paymentsService;
        private readonly IPaymentRepository paymentRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IUserBookRepository userBookRepository;
        private readonly BookService budgetService;

        public PaymentsController(
            PaymentsService paymentsService,
            IPaymentRepository paymentRepository,
            ICategoryRepository categoryRepository,
            IUserBookRepository userBookRepository,
            BookService budgetService)
        {
            this.paymentsService = paymentsService;
            this.paymentRepository = paymentRepository;
            this.categoryRepository = categoryRepository;
            this.userBookRepository = userBookRepository;
            this.budgetService = budgetService;
        }

        [HttpGet]
        public async Task<IActionResult> New(long accountId)
        {
            var categories = await categoryRepository.GetCategories(accountId);
            var users = await userBookRepository.GetUsers(accountId);
            var model = new PaymentViewModel()
            {
                Categories = categories.Select(x => new CategoryViewModel() { Id = x.Id, Name = x.Name }).ToList(),
                Users = users
            };

            model.Payment.Date = DateTime.Now;
            model.Payment.UserId = Guid.Parse(this.User.Claims.Single(x => x.Type == "Id").Value);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> New(long accountId, PaymentViewModel model)
        {
            paymentRepository.Add(
                new Payment()
                {
                    AccountId = accountId,
                    Amount = model.Payment.Amount,
                    Date = model.Payment.Date,
                    Description = model.Payment.Description,
                    CategoryId = model.Payment.CategoryId,
                    UserId = model.Payment.UserId
                });
            await paymentRepository.Save();

            return RedirectToAction("List");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(long id, long bookId)
        {
            var transaction = await paymentRepository.GetPayment(id);
            var categories = await categoryRepository.GetCategories(bookId);
            var users = await userBookRepository.GetUsers(bookId);
            var model = new PaymentViewModel()
            {
                Categories = categories.Select(x => new CategoryViewModel() { Id = x.Id, Name = x.Name }).ToList(),
                Payment = new PaymentModel(transaction),
                Users = users
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PaymentViewModel model)
        {
            await paymentsService.Save(new Payment(
                model.Payment.Id,
                model.Payment.Date,
                model.Payment.Description,
                model.Payment.Amount,
                model.Payment.CategoryId,
                model.Payment.UserId));
            return RedirectToAction("Index", "Dashboard");
        }


        [HttpGet]
        public async Task<IActionResult> Split(long id, long bookId)
        {
            var transaction = await paymentRepository.GetPayment(id);
            var categories = await categoryRepository.GetCategories(bookId);
            var users = await userBookRepository.GetUsers(bookId);
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
            var transaction = await paymentRepository.GetPayment(model.OldPaymentId);
            transaction.Amount = model.OldAmount;

            var newTransaction = new Payment()
            {
                AccountId = transaction.AccountId,
                Amount = model.NewAmount,
                CategoryId = model.CategoryId,
                Date = transaction.Date,
                Description = model.Description,
                UserId = model.OwnerId
            };

            paymentRepository.Add(newTransaction);
            await paymentRepository.Save();
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Remove([FromForm(Name = "Payment.Id")]long id)
        {
            await paymentRepository.Remove(id);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List(long accountId, int budgetId)
        {
            var interval = await budgetService.GetMonthlyBudgetById(accountId, budgetId);

            var transactions = await paymentsService.GetPayments(accountId, interval);
            var categories = await categoryRepository.GetCategories(accountId);
            var users = await userBookRepository.GetUsers(accountId);

            var model = new PaymentsViewModel()
            {
                Categories = categories.Select(x => new CategoryViewModel() { Id = x.Id, Name = x.Name }).ToList(),
                Payments = transactions.Select(x => new PaymentModel(x.Id, x.Date, x.Description, x.Amount, x.CategoryId, x.UserId)),
                Users = users
            };

            return View(model);
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

            public PaymentModel(long id, DateTime date, string description, decimal amount, long categoryId, Guid userId)
            {
                Id = id;
                Date = date;
                Description = description;
                Amount = amount;
                this.CategoryId = categoryId;
                this.UserId = userId;
            }

            public PaymentModel(Payment payment)
            {
                this.Id = payment.Id;
                this.Date = payment.Date;
                this.Description = payment.Description;
                this.Amount = payment.Amount;
                this.CategoryId = payment.CategoryId;
                this.UserId = payment.UserId;
            }

            public long Id { get; set; }
            public DateTime Date { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }
            public long CategoryId { get; set; }
            public Guid UserId { get; set; }
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
