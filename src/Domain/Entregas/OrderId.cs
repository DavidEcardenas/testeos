namespace LastMile.Domain.Entregas;

public sealed record OrderId(Guid Value)
{
    public static OrderId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();
}