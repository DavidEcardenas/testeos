using LastMile.Domain.Entregas;

namespace LastMile.Application.Entregas.Ports;

public interface IDeliveryRepository
{
    Task<Delivery?> GetAsync(DeliveryId id, CancellationToken ct);
    Task SaveAsync(Delivery delivery, CancellationToken ct);
}