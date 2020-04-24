﻿using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Budget.Domain
{
    public class ScheduledPaymentsService
    {
        private readonly IScheduledPaymentRepository repository;
        private readonly IPaymentRepository paymentRepository;

        public ScheduledPaymentsService(IScheduledPaymentRepository repository,
            IPaymentRepository paymentRepository)
        {
            this.repository = repository;
            this.paymentRepository = paymentRepository;
        }

        public Task<List<ScheduledPayment>> GetPayments(Guid userId)
        {
            return repository.GetPayments(userId);
        }

        public async Task Transform(long id, long accountId)
        {
            var scheduledPayment = await repository.GetPayment(id);
            var payment = new Payment()
            {
                AccountId = accountId,
                Amount = scheduledPayment.Amount,
                CategoryId = scheduledPayment.CategoryId,
                Date = scheduledPayment.Date,
                Description = scheduledPayment.Description,
                UserId = scheduledPayment.OwnerId
            };

            this.paymentRepository.Add(payment);
            await this.repository.Remove(id);
            await this.paymentRepository.Save();
        }
    }
}