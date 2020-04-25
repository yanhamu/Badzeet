using System;

namespace Badzeet.Scheduler.Domain.Model
{
    public class Log
    {
        public long Id { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public int RowsProcessed { get; set; }
    }
}