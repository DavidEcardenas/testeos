namespace LastMile.Application.Entregas;

public sealed record AssignCourierResult(
    Guid DeliveryId,
    Guid CourierId,
    DateTime AssignedAt);