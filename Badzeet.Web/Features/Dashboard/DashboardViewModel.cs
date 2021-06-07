using Badzeet.Budget.Domain;
using Badzeet.Web.Features.Common;
using System;
using System.Collections.Generic;

namespace Badzeet.Web.Features.Dashboard
{
    public class DashboardViewModel
    {
        public DateInterval Interval { get => new DateInterval(NavigationDates.Current.FirstBudgetDate, NavigationDates.Current.FirstBudgetDate.AddMonths(1).AddTicks(-1)); }
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public IDictionary<Guid, UserViewModel> Users { get; set; }
        public BudgetNavigationViewModel NavigationDates { get; set; }
        public decimal Total { get; set; }

        public DashboardViewModel(
            BudgetNavigationViewModel navigationDates,
            IEnumerable<CategoryViewModel> categories,
            IDictionary<Guid, UserViewModel> users,
            decimal total)
        {
            this.Categories = categories;
            this.Users = users;
            this.Total = total;
            this.NavigationDates = navigationDates;
        }
    }
}
