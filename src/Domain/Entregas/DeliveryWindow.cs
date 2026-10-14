namespace LastMile.Domain.Entregas;

public sealed record DeliveryWindow
{
    public DateTime Start { get; }
    public DateTime End { get; }

    public DeliveryWindow(DateTime start, DateTime end)
    {
        if (end <= start)
            throw new DomainException("Window end must be after start");

        Start = start;
        End = end;
    }

    public bool IsInThePast(DateTime now) => End < now;
}