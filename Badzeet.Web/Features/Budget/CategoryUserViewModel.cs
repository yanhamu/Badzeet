using System;

namespace Badzeet.Web.Features.Budget
{
    public class CategoryUserViewModel
    {
        public CategoryUserViewModel(Guid userId, string name, decimal total)
        {
            UserId = userId;
            Name = name;
            Total = total;
        }
        public Guid UserId { get; }
        public string Name { get; }
        public decimal Total { get; }
    }
}
