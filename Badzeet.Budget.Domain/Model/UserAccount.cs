﻿using System;

namespace Badzeet.Budget.Domain.Model
{
    public class UserAccount
    {
        public Guid UserId { get; set; }
        public Guid AccountId { get; set; }
        public User User { get; set; }
        public Account Account { get; set; }
    }
}