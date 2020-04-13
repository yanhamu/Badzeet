namespace Badzeet.Web.Features.Budget
{
    public class BudgetCategoryViewModel
    {
        public string Name { get; set; }
        public decimal Total { get; set; }
        public decimal Budget { get; set; }
        public CategoryUserViewModel[] Users { get; set; }
    }
}
