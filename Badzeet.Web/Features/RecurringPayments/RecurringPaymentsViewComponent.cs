using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.RecurringPayments
{
    public class RecurringPayments : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(Guid userId)
        {
            var payments = new List<RecurringPayment>() {
                new RecurringPayment(){ Description = "Netflix", Amount = 230m, CategoryName = "Entertaiment", Date = DateTime.Now },
                new RecurringPayment(){ Description = "Phone", Amount = 230m, CategoryName = "Others", Date = DateTime.Now }
            };

            return Task.FromResult((IViewComponentResult)View(payments));
        }

        public class RecurringPayment
        {
            public long Id { get; set; }
            public long AccountId { get; set; }
            public DateTime Date { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }
            public long CategoryId { get; set; }
            public string CategoryName { get; set; }
            public Guid OwnerId { get; set; }
            public string Owner { get; set; }

        }
    }
}