using LastMile.Domain.Entregas;

namespace LastMile.Application.Entregas.Ports;

public interface IStockReservationPort
{
    Task<bool> IsReservedAsync(OrderId orderId, CancellationToken ct);
}