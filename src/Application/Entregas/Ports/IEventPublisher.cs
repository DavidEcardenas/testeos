namespace LastMile.Application.Entregas.Ports;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct)
        where TEvent : class;
}