using LastMile.Application.Entregas.Ports;

namespace LastMile.Application.Tests.Fakes;

public sealed class FakeEventPublisher : IEventPublisher
{
    public List<object> Published { get; } = new();

    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct)
        where TEvent : class
    {
        Published.Add(@event);
        return Task.CompletedTask;
    }
}