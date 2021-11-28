using System;

namespace Badzeet.Budget.Domain
{
    public class OpenDateInterval
    {
        public OpenDateInterval(DateTime? from, DateTime? to)
        {
            From = from;
            To = to;
        }

        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
