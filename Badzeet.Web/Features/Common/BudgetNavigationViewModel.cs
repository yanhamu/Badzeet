using System;

namespace Badzeet.Web.Features.Common
{
    public class BudgetNavigationViewModel
    {
        public BudgetNavigationItemViewModel Previous { get; set; }
        public BudgetNavigationItemViewModel Current { get; set; }
        public BudgetNavigationItemViewModel Next { get; set; }
    }

    public class BudgetNavigationItemViewModel
    {
        public DateTime FirstBudgetDate { get; set; }
        public long? BudgetId { get; set; }
    }
}
