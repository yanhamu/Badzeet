using Badzeet.Scheduler.Domain.Interfaces;
using Badzeet.Scheduler.Domain.Model;

namespace Badzeet.Scheduler.DataAccess
{
    public class LogRepository : ILogRepository
    {
        private readonly SchedulerDbContext context;

        public LogRepository(SchedulerDbContext context)
        {
            this.context = context;
        }
        public void Add(Log log)
        {
            context.Set<Log>().Add(log);
        }
    }
}
