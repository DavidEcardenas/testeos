namespace LastMile.Domain.Entregas;

public sealed class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}