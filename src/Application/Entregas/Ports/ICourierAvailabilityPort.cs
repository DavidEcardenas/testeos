using LastMile.Domain.Entregas;

namespace LastMile.Application.Entregas.Ports;

public interface ICourierAvailabilityPort
{
    Task<bool> IsAvailableAsync(CourierId id, CancellationToken ct);
}