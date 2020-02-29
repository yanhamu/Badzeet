using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.Book
{
    [Authorize]
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public Task<IActionResult> List()
        {
            var transactions = new List<TransactionModel>() {
            new TransactionModel(9,new DateTime(2020,1,8), "Lunch", 165m),
            new TransactionModel(8,new DateTime(2020,1,7), "Billa", 687),
            new TransactionModel(7,new DateTime(2020,1,6), "Meat", 98m),
            new TransactionModel(6,new DateTime(2020,1,5), "Lunch", 160m),
            new TransactionModel(5,new DateTime(2020,1,4), "Pills", 416m),
            new TransactionModel(4,new DateTime(2020,1,3), "Lunch", 220m),
            new TransactionModel(3,new DateTime(2020,1,2), "Dinner", 180),
            new TransactionModel(2,new DateTime(2020,1,2), "Chewing gum", 16),
            new TransactionModel(1,new DateTime(2020,1,1), "Billa", 200m),
            };
            var model = new TransactionsModel() { Transactions = transactions };

            return Task.FromResult<IActionResult>(View(model));
        }
    }
}