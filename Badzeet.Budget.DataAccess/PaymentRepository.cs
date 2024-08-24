using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Badzeet.Budget.DataAccess;

public class PaymentRepository : IPaymentRepository
{
    private readonly BudgetDbContext context;

    public PaymentRepository(BudgetDbContext context)
    {
        this.context = context;
    }

    public Task<Payment?> Get(long id)
    {
        return context.Set<Payment>().SingleOrDefaultAsync(x => x.Id == id);
    }

    public Payment Add(Payment payment)
    {
        return context.Set<Payment>().Add(payment).Entity;
    }

    public async Task<Payment?> Remove(long id)
    {
        var payment = await context.Set<Payment>().FindAsync(id);
        if (payment == null)
            return default;

        context.Set<Payment>().Remove(payment);
        return payment;
    }

    public Task<Payment?> GetLastPayment(long accountId)
    {
        return context
            .Set<Payment>()
            .Where(x => x.AccountId == accountId)
            .OrderByDescending(x => x.Date)
            .FirstOrDefaultAsync();
    }

    public Task Save()
    {
        return context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Payment>> GetPayments(PaymentsFilter filter)
    {
        var baseQuery = context.Set<Payment>().Where(x => x.AccountId == filter.AccountId);

        if (filter.UserId.HasValue)
            baseQuery = baseQuery.Where(x => x.UserId == filter.UserId.Value);

        if (filter.PaymentType.HasValue)
            baseQuery = baseQuery.Where(x => x.Type == filter.PaymentType.Value);

        if (filter.Interval.From.HasValue)
            baseQuery = baseQuery.Where(x => x.Date >= filter.Interval.From.Value);

        if (filter.Interval.To.HasValue)
            baseQuery = baseQuery.Where(x => x.Date <= filter.Interval.To.Value);

        if (filter.CategoryIds.Any())
            baseQuery = baseQuery.Where(x => filter.CategoryIds.Contains(x.CategoryId));

        return await baseQuery.OrderByDescending(x => x.Date).ToListAsync();
    }
}