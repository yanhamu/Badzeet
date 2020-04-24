using System;

namespace Badzeet.Domain.Budget
{
    public struct DateInterval
    {
        public DateTime From { get; }
        public DateTime To { get; }

        public DateInterval(DateTime from, DateTime to)
        {
            if (from > to)
                throw new ArgumentException("from cannot be higher than to");

            From = from.Date;
            To = to.Date;
        }

        public DateInterval AddMonth(int months)
        {
            return new DateInterval(From.AddMonths(months), To.AddMonths(months));
        }
    }
}
