using LastMile.Domain.Entregas;

namespace LastMile.Application.Entregas.Ports;

public interface IRouteCapacityPort
{
    Task<bool> HasCapacityAsync(CourierId id, CancellationToken ct);
}