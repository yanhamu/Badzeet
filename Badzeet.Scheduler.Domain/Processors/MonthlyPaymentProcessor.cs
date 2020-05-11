using Badzeet.Integration.Events;
using Badzeet.Scheduler.Domain.Model;
using MediatR;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Badzeet.Scheduler.Domain.Processors
{
    public class MonthlyPaymentProcessor : IProcessor
    {
        private readonly IMediator mediator;

        public MonthlyPaymentProcessor(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public SchedulingType Id => SchedulingType.Monthly;

        public async Task Process(Payment payment, DateTime now)
        {
            payment.ScheduledAt = CalculateNewSchedule(payment.Metadata, payment.ScheduledAt);
            payment.UpdatedAt = DateTime.UtcNow;

            await mediator.Send(new NewScheduledPaymentRequest(payment.AccountId, now, payment.Amount, payment.Description, payment.CategoryId, payment.OwnerId));
        }

        public DateTime CalculateNewSchedule(string metadata, DateTime referenceDate)
        {
            var settings = MonthlySettings.Parse(metadata);
            if (settings.LastDay)
            {
                var d = new DateTime(referenceDate.Year, referenceDate.Month, 1).AddMonths(2).AddDays(-1);
                return new DateTime(d.Year, d.Month, d.Day, settings.When.Hours, settings.When.Minutes, settings.When.Seconds);
            }
            else
            {
                var d = referenceDate.AddMonths(1);
                return new DateTime(d.Year, d.Month, settings.Day.Value, settings.When.Hours, settings.When.Minutes, settings.When.Seconds);
            }
        }

        public class MonthlySettings
        {
            public bool LastDay { get; set; }
            public int? Day { get; set; }
            public TimeSpan When { get; set; }

            public MonthlySettings() { }

            private MonthlySettings(bool lastDay, int? day, TimeSpan when)
            {
                this.LastDay = lastDay;
                this.Day = day;
                this.When = when;
            }

            public static MonthlySettings Parse(string serialized)
            {
                return JsonSerializer.Deserialize<MonthlySettings>(serialized);
            }

            public string Serialize()
            {
                return JsonSerializer.Serialize(this);
            }

            public static MonthlySettings CreateLastDayOfTheMonth(TimeSpan when)
            {
                return new MonthlySettings(true, null, when);
            }

            public static MonthlySettings CreateFixedDay(int day, TimeSpan when)
            {
                return new MonthlySettings(false, day, when);
            }
        }
    }
}