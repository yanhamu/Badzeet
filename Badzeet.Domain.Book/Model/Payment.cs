﻿using System;

namespace Badzeet.Domain.Budget.Model
{
    public class Payment
    {
        public Payment()
        {

        }

        public Payment(long id, DateTime date, string description, decimal amount, long categoryId, Guid userId)
        {
            Id = id;
            Date = date;
            Description = description;
            Amount = amount;
            CategoryId = categoryId;
            UserId = userId;
        }

        public long Id { get; set; }
        public long AccountId { get; set; }
        public Account Account { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}