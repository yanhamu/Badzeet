namespace Badzeet.Web.Features.Budget
{
    public class CategoryUserViewModel
    {
        public CategoryUserViewModel(string name, decimal total)
        {
            Name = name;
            Total = total;
        }

        public string Name { get; }
        public decimal Total { get; }
    }
}
