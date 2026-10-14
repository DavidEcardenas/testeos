namespace LastMile.Application.Entregas;

public sealed record DeliveryAssigned(
    Guid DeliveryId,
    Guid CourierId,
    DateTime OccurredAt);