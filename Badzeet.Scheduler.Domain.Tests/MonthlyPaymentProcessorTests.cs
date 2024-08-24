using Badzeet.Scheduler.Domain.Processors;

namespace Badzeet.Scheduler.Domain.Tests;

public class MonthlyPaymentProcessorTests
{
    [Fact]
    public void DeserializeTest()
    {
        var serialized = @"{
            ""LastDay"": false,
            ""Day"": 4,
            ""When"": ""16:15:20""
            }";
        var settings = MonthlyPaymentProcessor.MonthlySettings.Parse(serialized);

        Assert.Equal(16, settings.When.Hours);
        Assert.Equal(15, settings.When.Minutes);
        Assert.Equal(20, settings.When.Seconds);
    }

    [Fact]
    public void SerializeTest()
    {
        var settings = new MonthlyPaymentProcessor.MonthlySettings
        {
            Day = 5,
            LastDay = false,
            When = TimeSpan.FromHours(5)
        };
        var serialized = settings.Serialize();
    }
}