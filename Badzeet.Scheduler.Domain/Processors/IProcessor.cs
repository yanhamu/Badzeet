using Badzeet.Scheduler.Domain.Model;
using System;
using System.Threading.Tasks;

namespace Badzeet.Scheduler.Domain.Processors
{
    public interface IProcessor
    {
        SchedulingType Id { get; }
        Task Process(Payment payment, DateTime now);
    }
}