using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace TestHarnessFreeze;

public class TestHarnessShould
{
    
    [Fact]
    public async Task PublishFilter_OnPublish_HeadersSetUpCorrectly()
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
        
        var events = ToArray<FooEvent>(harness);

        // Assert
        (await harness.Published.Any<FooEvent>(x => x.Context.Message.MessageId == fooEvent.MessageId)).Should().BeTrue();
    }

    private static T[] ToArray<T>(ITestHarness harness) where T : IntegrationMessage
    {
        return harness.Published.Select(x => x.MessageObject.GetType() == typeof(T))
            .Select(x => (T)x.MessageObject)
            .ToArray();
    }
}