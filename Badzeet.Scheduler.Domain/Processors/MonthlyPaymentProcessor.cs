using Badzeet.Scheduler.Domain.Model;
using System;
using System.Threading.Tasks;

namespace Badzeet.Scheduler.Domain.Processors
{
    public class MonthlyPaymentProcessor : IProcessor
    {
        private readonly PaymentBus bus;

        public MonthlyPaymentProcessor()
        {
            this.bus = new PaymentBus();
        }

        public SchedulingType Id => SchedulingType.Monthly;

        public async Task Process(Payment payment, DateTime now)
        {
            payment.ScheduledAt = CalculateNewSchedule(payment.Metadata, now);
            payment.UpdatedAt = DateTime.UtcNow;

            await bus.Publish(new PaymentDto(payment.AccountId, payment.Date, payment.Amount, payment.Description, payment.CategoryId, payment.OwnerId));
        }

        private DateTime CalculateNewSchedule(string metadata, DateTime now)
        {
            var settings = MonthlySettings.Parse(metadata);
            if (settings.LastDay)
            {
                var d = now.AddMonths(1);
                return new DateTime(d.Year, d.Month, d.Day, settings.When.Hours, settings.When.Minutes, settings.When.Seconds);
            }
            else
            {
                var d = now.AddMonths(1);
                return new DateTime(d.Year, d.Month, settings.Day.Value, settings.When.Hours, settings.When.Minutes, settings.When.Seconds);
            }
        }

        private class MonthlySettings
        {
            public bool LastDay { get; set; }
            public int? Day { get; set; }
            public TimeSpan When { get; set; }

            private MonthlySettings(bool lastDay, int? day, TimeSpan when)
            {
                this.LastDay = lastDay;
                this.Day = day;
                this.When = when;
            }

            public static MonthlySettings Parse(string serialized)
            {
                return System.Text.Json.JsonSerializer.Deserialize<MonthlySettings>(serialized);
            }

            public string Serialize()
            {
                return System.Text.Json.JsonSerializer.Serialize(this);
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