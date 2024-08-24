using System;

namespace Badzeet.Budget.Domain;

public class OpenDateInterval(DateTime? from, DateTime? to)
{
    public DateTime? From { get; set; } = from;
    public DateTime? To { get; set; } = to;
}