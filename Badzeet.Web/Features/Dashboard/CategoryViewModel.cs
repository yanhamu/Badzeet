using System;
using System.Collections.Generic;

namespace Badzeet.Web.Features.Dashboard
{
    public class CategoryViewModel
    {
        public CategoryViewModel(
            Guid id,
            string name,
            IDictionary<Guid, decimal> perUserSum,
            decimal sum)
        {
            Id = id;
            Name = name;
            PerUserSum = perUserSum;
            Sum = sum;
        }

        public Guid Id { get; }
        public string Name { get; }

        public decimal Sum { get; }
        public IDictionary<Guid, decimal> PerUserSum { get; }
    }
}