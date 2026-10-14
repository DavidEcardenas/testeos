using LastMile.Application.Entregas.Ports;
using LastMile.Domain.Entregas;

namespace LastMile.Application.Tests.Fakes;

public sealed class FakeDeliveryRepository : IDeliveryRepository
{
    private readonly Dictionary<Guid, Delivery> _store = new();

    public void Seed(Delivery delivery) => _store[delivery.Id.Value] = delivery;

    public Task<Delivery?> GetAsync(DeliveryId id, CancellationToken ct)
    {
        _store.TryGetValue(id.Value, out var delivery);
        return Task.FromResult(delivery);
    }

    public Task SaveAsync(Delivery delivery, CancellationToken ct)
    {
        _store[delivery.Id.Value] = delivery;
        return Task.CompletedTask;
    }
}