using Badzeet.Scheduler.Domain.Model;

namespace Badzeet.Scheduler.Domain.Interfaces;

public interface ILogRepository
{
    void Add(Log log);
}