﻿using Badzeet.Domain.Book;
using Badzeet.Domain.Book.Interfaces;
using Badzeet.Domain.Book.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.Book
{
    [Authorize]
    public partial class BookController : Controller
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
        public async Task<IActionResult> List(long bookId, int budgetId)
        {
            var interval = await budgetService.GetMonthlyBudgetById(bookId, budgetId);

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
        public async Task<IActionResult> Split(long id, long bookId)
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
        public async Task<IActionResult> Split(SplitModel model)
        {
            var transaction = await transactionRepository.GetTransaction(model.OldTransactionId);
            transaction.Amount = model.OldAmount;

            var newTransaction = new Transaction()
            {
                AccountId = transaction.AccountId,
                Amount = model.NewAmount,
                CategoryId = model.CategoryId,
                Date = transaction.Date,
                Description = model.Description,
                UserId = model.OwnerId
            };

            transactionRepository.Add(newTransaction);
            await transactionRepository.Save();
            return RedirectToAction("List");
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
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> NewRecord(long accountId)
        {
            var categories = await categoryRepository.GetCategories(accountId);
            var users = await userBookRepository.GetUsers(accountId);
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
        public async Task<IActionResult> NewRecord(long accountId, TransactionViewModel model)
        {
            transactionRepository.Add(
                new Transaction()
                {
                    AccountId = accountId,
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
}