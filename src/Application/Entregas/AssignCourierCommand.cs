namespace LastMile.Application.Entregas;

public sealed record AssignCourierCommand(
    Guid DeliveryId,
    Guid CourierId,
    string RequestedBy);