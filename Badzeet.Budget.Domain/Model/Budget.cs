using System;

namespace Badzeet.Budget.Domain.Model;

public class Budget
{
    public Guid Id { get; set; }
    public int BudgetId { get; set; }
    public long AccountId { get; set; }
    public Account Account { get; set; } = default!;
    
    /// <summary>
    /// Inclusive Date
    /// </summary>
    public DateTime DateFrom { get; set; }
    
    /// <summary>
    /// Exclusive Date 
    /// </summary>
    public DateTime DateTo { get; set; }
    public DateInterval Interval => new(DateFrom, DateTo);
}