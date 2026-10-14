using LastMile.Application.Entregas.Ports;
using LastMile.Domain.Entregas;

namespace LastMile.Application.Tests.Fakes;

public sealed class FakeCourierAvailabilityPort : ICourierAvailabilityPort
{
    public bool Available { get; set; } = true;

    public Task<bool> IsAvailableAsync(CourierId id, CancellationToken ct)
        => Task.FromResult(Available);
}