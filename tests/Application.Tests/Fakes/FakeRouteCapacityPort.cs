using LastMile.Application.Entregas.Ports;
using LastMile.Domain.Entregas;

namespace LastMile.Application.Tests.Fakes;

public sealed class FakeRouteCapacityPort : IRouteCapacityPort
{
    public bool HasCapacity { get; set; } = true;

    public Task<bool> HasCapacityAsync(CourierId id, CancellationToken ct)
        => Task.FromResult(HasCapacity);
}