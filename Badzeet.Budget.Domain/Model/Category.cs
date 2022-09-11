using System;

namespace Badzeet.Budget.Domain.Model
{
    public class Category
    {
        public Guid Id { get; set; }
        public long AccountId { get; set; }
        public Account Account { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
    }
}