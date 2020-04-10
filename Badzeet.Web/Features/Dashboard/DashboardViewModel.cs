using Badzeet.Domain.Book;
using System;
using System.Collections.Generic;

namespace Badzeet.Web.Features.Dashboard
{
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
