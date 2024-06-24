using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace TestHarnessFreeze;

public class TestHarnessShould
{
    
    [Fact]
    public async Task TestHarnessShould_GetEventsOfTypeTCorrectly()
    {
        // Arrange
        await using var provider = new ServiceCollection()
            .AddMassTransitTestHarness(x =>
            {
                x.AddConsumer<FooEventConsumer>();
            })
            .BuildServiceProvider(true);

        var harness = provider.GetRequiredService<ITestHarness>();

        await harness.Start();
        
        var fooEvent = new FooEvent { MessageId = 1};


        // Act
        await harness.Start();
        await harness.Bus.Publish(fooEvent);

        var events = harness.Published
            .Select(x => x.MessageObject.GetType() == typeof(FooEvent))
            .Select(x => (FooEvent)x.MessageObject)
            .ToArray();
        
        // Assert
        (events.First().MessageId == fooEvent.MessageId).Should().BeTrue();
    }
}