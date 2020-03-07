﻿using Badzeet.Domain.Book.Interfaces;
using Badzeet.Domain.Book.Model;
using Badzeet.Web.Configuration;
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

        public BookController(
            TransactionsService transactionsService,
            ITransactionRepository transactionRepository,
            ICategoryRepository categoryRepository)
        {
            this.transactionsService = transactionsService;
            this.transactionRepository = transactionRepository;
            this.categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var from = DateTime.Now.AddYears(-2).AddMonths(-1);
            var to = DateTime.Now.AddYears(-2);
            var bookId = this.HttpContext.GetBookId();

            var transactions = await transactionsService.GetTransactions(bookId, from, to);
            var categories = await categoryRepository.GetCategories(bookId);
            var model = new TransactionsModel()
            {
                Categories = categories.Select(x => new CategoryModel() { Id = x.Id, Name = x.Name }).ToList(),
                Transactions = transactions.Select(x => new TransactionModel(x.Id, x.Date, x.Description, x.Amount, x.CategoryId))
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditRecord(long id)
        {
            var transaction = await transactionRepository.GetTransaction(id);
            var categories = await categoryRepository.GetCategories(this.HttpContext.GetBookId());

            var model = new EditModel()
            {
                Categories = categories.Select(x => new CategoryModel() { Id = x.Id, Name = x.Name }).ToList(),
                Transaction = new TransactionModel(transaction)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRecord(EditModel model)
        {
            await transactionsService.Save(new Transaction(
                model.Transaction.Id,
                model.Transaction.Date,
                model.Transaction.Description,
                model.Transaction.Amount,
                model.Transaction.CategoryId));
            return LocalRedirect("/Book/List");
        }

        [HttpGet]
        public async Task<IActionResult> NewRecord()
        {
            var categories = await categoryRepository.GetCategories(this.HttpContext.GetBookId());
            var model = new EditModel()
            {
                Categories = categories.Select(x => new CategoryModel() { Id = x.Id, Name = x.Name }).ToList(),
            };

            model.Transaction.Date = DateTime.Now;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> NewRecord(EditModel model)
        {
            transactionRepository.Add(
                new Transaction()
                {
                    BookId = HttpContext.GetBookId(),
                    Amount = model.Transaction.Amount,
                    Date = model.Transaction.Date,
                    Description = model.Transaction.Description,
                    CategoryId = model.Transaction.CategoryId
                });
            await transactionRepository.Save();

            return LocalRedirect("/Book/List");
        }
    }

    public class EditModel
    {
        public List<CategoryModel> Categories { get; set; } = new List<CategoryModel>();
        public TransactionModel Transaction { get; set; } = new TransactionModel();
    }
}