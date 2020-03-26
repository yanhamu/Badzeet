using Badzeet.Domain.Book;
using Badzeet.Domain.Book.Interfaces;
using Badzeet.Domain.Book.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.Book
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly TransactionsService transactionsService;
        private readonly ITransactionRepository transactionRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IUserBookRepository userBookRepository;
        private readonly BookService budgetService;

        public BookController(
            TransactionsService transactionsService,
            ITransactionRepository transactionRepository,
            ICategoryRepository categoryRepository,
            IUserBookRepository userBookRepository,
            BookService budgetService)
        {
            this.transactionsService = transactionsService;
            this.transactionRepository = transactionRepository;
            this.categoryRepository = categoryRepository;
            this.userBookRepository = userBookRepository;
            this.budgetService = budgetService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(long bookId, int budgetId)
        {
            var interval = await budgetService.GetBudgetByOffset(bookId, budgetId);
            var allCategories = await categoryRepository.GetCategories(bookId);
            var transactions = await transactionRepository.GetTransactions(bookId, interval);
            var allUsers = await userBookRepository.GetUsers(bookId);

            var categories = new List<CategoryViewModel>();
            var total = 0m;
            var users = new Dictionary<Guid, UserViewModel>();

            foreach (var category in allCategories)
            {
                var categoryTransactions = transactions.Where(x => x.CategoryId == category.Id).ToArray();
                var totalSum = 0m;
                var perUserSum = new Dictionary<Guid, decimal>();
                foreach (var user in allUsers)
                {
                    var sum = categoryTransactions
                        .Where(x => x.UserId == user.UserId)
                        .Sum(x => x.Amount);

                    perUserSum.Add(user.UserId, sum);

                    totalSum += sum;
                    total += sum;

                    if (users.ContainsKey(user.UserId) == false)
                        users.Add(user.UserId, new UserViewModel(user.UserId, user.Nickname, 0));

                    users[user.UserId].Total += sum;
                }
                var c = new CategoryViewModel(category.Id, category.Name, perUserSum, totalSum);
                categories.Add(c);
            }

            var model = new DashboardViewModel(
                budgetId,
                interval,
                categories,
                users,
                total);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> List(long bookId, int budgetId)
        {
            var interval = await budgetService.GetBudgetByOffset(bookId, budgetId);//.GetLatestBudget(bookId);

            var transactions = await transactionsService.GetTransactions(bookId, interval);
            var categories = await categoryRepository.GetCategories(bookId);
            var users = await userBookRepository.GetUsers(bookId);

            var model = new TransactionsModel()
            {
                Categories = categories.Select(x => new CategoryModel() { Id = x.Id, Name = x.Name }).ToList(),
                Transactions = transactions.Select(x => new TransactionModel(x.Id, x.Date, x.Description, x.Amount, x.CategoryId, x.UserId)),
                Users = users
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditRecord(long id, long bookId)
        {
            var transaction = await transactionRepository.GetTransaction(id);
            var categories = await categoryRepository.GetCategories(bookId);
            var users = await userBookRepository.GetUsers(bookId);
            var model = new TransactionViewModel()
            {
                Categories = categories.Select(x => new CategoryModel() { Id = x.Id, Name = x.Name }).ToList(),
                Transaction = new TransactionModel(transaction),
                Users = users
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SplitRecord(long id, long bookId)
        {
            var transaction = await transactionRepository.GetTransaction(id);
            var categories = await categoryRepository.GetCategories(bookId);
            var users = await userBookRepository.GetUsers(bookId);
            var model = new TransactionViewModel()
            {
                Categories = categories.Select(x => new CategoryModel() { Id = x.Id, Name = x.Name }).ToList(),
                Transaction = new TransactionModel(transaction),
                Users = users
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRecord(TransactionViewModel model)
        {
            await transactionsService.Save(new Transaction(
                model.Transaction.Id,
                model.Transaction.Date,
                model.Transaction.Description,
                model.Transaction.Amount,
                model.Transaction.CategoryId,
                model.Transaction.UserId));
            return LocalRedirect("/Book/List");
        }

        [HttpGet]
        public async Task<IActionResult> NewRecord(long bookId)
        {
            var categories = await categoryRepository.GetCategories(bookId);
            var users = await userBookRepository.GetUsers(bookId);
            var model = new TransactionViewModel()
            {
                Categories = categories.Select(x => new CategoryModel() { Id = x.Id, Name = x.Name }).ToList(),
                Users = users
            };

            model.Transaction.Date = DateTime.Now;
            model.Transaction.UserId = Guid.Parse(this.User.Claims.Single(x => x.Type == "Id").Value);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> NewRecord(TransactionViewModel model, long bookId)
        {
            transactionRepository.Add(
                new Transaction()
                {
                    BookId = bookId,
                    Amount = model.Transaction.Amount,
                    Date = model.Transaction.Date,
                    Description = model.Transaction.Description,
                    CategoryId = model.Transaction.CategoryId,
                    UserId = model.Transaction.UserId
                });
            await transactionRepository.Save();

            return LocalRedirect("/Book/List");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRecord([FromForm(Name = "Transaction.Id")]long id)
        {
            await transactionRepository.Remove(id);
            return LocalRedirect("/Book/List");
        }
    }

    public class TransactionViewModel
    {
        public List<CategoryModel> Categories { get; set; } = new List<CategoryModel>();
        public TransactionModel Transaction { get; set; } = new TransactionModel();
        public IEnumerable<UserBook> Users { get; set; } = new List<UserBook>();
    }

    public class CategoryViewModel
    {
        public CategoryViewModel(
            long id,
            string name,
            IDictionary<Guid, decimal> perUserSum,
            decimal sum)
        {
            Id = id;
            Name = name;
            PerUserSum = perUserSum;
            Sum = sum;
        }

        public long Id { get; }
        public string Name { get; }

        public decimal Sum { get; }
        public IDictionary<Guid, decimal> PerUserSum { get; }
    }

    public class UserViewModel
    {
        public UserViewModel(Guid id, string nickname, decimal total)
        {
            Id = id;
            Nickname = nickname;
            Total = total;
        }

        public Guid Id { get; }
        public string Nickname { get; }
        public decimal Total { get; set; }
    }

    public class DashboardViewModel
    {
        public int BudgetId { get; set; }
        public DateInterval Interval { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public IDictionary<Guid, UserViewModel> Users { get; set; }
        public decimal Total { get; set; }

        public DashboardViewModel(
            int budgetId,
            DateInterval interval,
            IEnumerable<CategoryViewModel> categories,
            IDictionary<Guid, UserViewModel> users,
            decimal total)
        {
            this.Interval = interval;
            this.Categories = categories;
            this.Users = users;
            this.Total = total;
            this.BudgetId = budgetId;
        }
    }
}