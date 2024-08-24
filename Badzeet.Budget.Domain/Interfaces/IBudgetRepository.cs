using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Budget.Domain.Interfaces;

public interface IBudgetRepository
{
    Model.Budget Create(Model.Budget budget);
    Task<List<Model.Budget>> List(long accountId, Filter filter);
    Task Save();
    ValueTask<Model.Budget?> Get(int budgetId, long accountId);
}

public class Filter
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}