﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Badzeet.Integration.Events;
using MediatR;

namespace Badzeet.Budget.Domain;

public class PaymentsService(IPaymentRepository repository, ICategoryRepository categoryRepository)
    : IRequestHandler<NewScheduledPaymentRequest>
{
    public async Task Handle(NewScheduledPaymentRequest request, CancellationToken cancellationToken)
    {
        var category = (await categoryRepository.Get(request.CategoryId))!;
        repository.Add(new Payment(request.Date, request.Description, request.Amount, category.Id, request.OwnerId, PaymentType.Pending, request.AccountId));
        await repository.Save();
    }

    public Task<Payment?> GetPayment(long id)
    {
        return repository.Get(id);
    }

    public Task Add(Payment payment)
    {
        repository.Add(payment);
        return repository.Save();
    }

    public async Task Remove(long id)
    {
        await repository.Remove(id);
        await repository.Save();
    }

    public async Task Save(Payment payment)
    {
        var tracked = (await repository.Get(payment.Id))!;
        tracked.Date = payment.Date;
        tracked.Description = payment.Description;
        tracked.Amount = payment.Amount;
        tracked.CategoryId = payment.CategoryId;
        tracked.UserId = payment.UserId;
        tracked.Type = payment.Type;
        await repository.Save();
    }

    public async Task Transform(long id)
    {
        var scheduledPayment = (await repository.Get(id))!;
        scheduledPayment.Date = DateTime.UtcNow;
        scheduledPayment.Type = PaymentType.Normal;
        await repository.Save();
    }

    public async Task Split(long oldPaymentId, decimal oldAmount, string description, decimal newAmount, long categoryId, Guid ownerId)
    {
        var payment = (await repository.Get(oldPaymentId))!;
        payment.Amount = oldAmount;
        var newPayment = new Payment(payment.Date, description, newAmount, categoryId, ownerId, PaymentType.Normal, payment.AccountId);
        repository.Add(newPayment);
        await repository.Save();
    }
}