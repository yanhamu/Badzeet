using Badzeet.Domain.Book;
using System.Collections.Generic;

namespace Badzeet.Web.Features
{
    public class DashboardViewModel
    {
        public DateInterval Interval;

        public IEnumerable<CategoryTuple> Categories { get; set; }

        public DashboardViewModel(DateInterval interval, IEnumerable<CategoryTuple> categories)
        {
            this.Interval = interval;
            this.Categories = categories;
        }
    }
}