using System;
using System.Threading.Tasks;
using Badzeet.Scheduler.Domain.Model;

namespace Badzeet.Scheduler.Domain.Processors;

public interface IProcessor
{
    SchedulingType Id { get; }
    Task Process(Payment payment, DateTime now);
}