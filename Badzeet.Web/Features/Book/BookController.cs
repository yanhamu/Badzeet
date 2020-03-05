using Badzeet.Domain.Book.Interfaces;
using Badzeet.Domain.Book.Model;
using Badzeet.Web.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.Book
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly TransactionsService transactionsService;
        private readonly ITransactionRepository transactionRepository;

        public BookController(TransactionsService transactionsService, ITransactionRepository transactionRepository)
        {
            this.transactionsService = transactionsService;
            this.transactionRepository = transactionRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> List()
        {
            var from = DateTime.Now.AddYears(-2).AddMonths(-1);
            var to = DateTime.Now.AddYears(-2);

            var transactions = await transactionsService.GetTransactions(this.HttpContext.GetAccountId(), from, to);

            var model = new TransactionsModel() { Transactions = transactions.Select(x => new TransactionModel(x.Id, x.Date, x.Description, x.Amount)) };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditRecord(long id)
        {
            var transaction = await transactionRepository.GetTransaction(id);

            var model = new EditModel()
            {
                Transaction = new TransactionModel(transaction)
            };

            return View(model);
        }

        [HttpPost]
        public Task<IActionResult> EditRecord(EditModel model)
        {
            return Task.FromResult<IActionResult>(LocalRedirect("/Book/List"));
        }

        [HttpGet]
        public IActionResult NewRecord()
        {
            var model = new EditModel();
            model.Transaction.Date = DateTime.Now;
            return View(model);
        }

        [HttpPost]
        public Task<IActionResult> NewRecord(EditModel model)
        {
            return Task.FromResult<IActionResult>(LocalRedirect("/Book/List"));
        }
    }

    public class EditModel
    {
        public TransactionModel Transaction { get; set; } = new TransactionModel();
    }
}