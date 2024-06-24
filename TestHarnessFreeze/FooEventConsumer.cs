using MassTransit;

namespace TestHarnessFreeze;

public class FooEventConsumer : IConsumer<FooEvent>
{
    public Task Consume(ConsumeContext<FooEvent> context)
    {
        return Task.CompletedTask;
    }
}