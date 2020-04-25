using Badzeet.Scheduler.Domain.Interfaces;
using Badzeet.Scheduler.Domain.Model;
using Badzeet.Scheduler.Domain.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Scheduler.Domain
{
    public class PaymentScheduler
    {
        private readonly IEnumerable<IProcessor> processors;
        private readonly IPaymentRepository repository;
        private readonly ILogRepository logRepository;

        public PaymentScheduler(IEnumerable<IProcessor> processors, IPaymentRepository paymentRepository, ILogRepository logRepository)
        {
            this.processors = processors;
            this.repository = paymentRepository;
            this.logRepository = logRepository;
        }

        public async Task Run()
        {
            var now = DateTime.UtcNow;
            var payments = await repository.FetchAllToProcess(now);

            foreach (var payment in payments)
            {
                var processor = processors.Single(x => x.Id == payment.SchedulingType);
                await processor.Process(payment, now);
            }

            logRepository.Add(new Log() { StartedAt = now, FinishedAt = DateTime.UtcNow, RowsProcessed = payments.Count });
            await repository.SaveAll();
        }
    }
}