using LastMile.Application.Entregas.Ports;
using LastMile.Domain.Entregas;

namespace LastMile.Application.Tests.Fakes;

public sealed class FakeStockReservationPort : IStockReservationPort
{
    public bool Reserved { get; set; } = true;

    public Task<bool> IsReservedAsync(OrderId orderId, CancellationToken ct)
        => Task.FromResult(Reserved);
}