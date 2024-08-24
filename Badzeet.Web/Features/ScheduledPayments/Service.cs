using System.Collections.Generic;
using System.Threading.Tasks;
using Badzeet.Scheduler.Domain.Interfaces;
using Badzeet.Scheduler.Domain.Model;

namespace Badzeet.Web.Features.ScheduledPayments;

public class Service
{
    private readonly IPaymentRepository repository;

    public Service(IPaymentRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<Payment>> List(long accountId)
    {
        return await repository.GetPayments(accountId);
    }
}