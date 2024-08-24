using System;
using System.Collections.Generic;
using Badzeet.Budget.Domain;
using Badzeet.Web.Features.Common;

namespace Badzeet.Web.Features.Dashboard;

public class DashboardViewModel
{
    public DashboardViewModel(
        BudgetNavigationViewModel navigationDates,
        IEnumerable<CategoryViewModel> categories,
        IDictionary<Guid, UserViewModel> users,
        decimal total)
    {
        Categories = categories;
        Users = users;
        Total = total;
        NavigationDates = navigationDates;
    }

    public DateInterval Interval => new(NavigationDates.Current.FirstBudgetDate, NavigationDates.Current.FirstBudgetDate.AddMonths(1).AddTicks(-1));
    public IEnumerable<CategoryViewModel> Categories { get; set; }
    public IDictionary<Guid, UserViewModel> Users { get; set; }
    public BudgetNavigationViewModel NavigationDates { get; set; }
    public decimal Total { get; set; }
}