namespace LastMile.Domain.Entregas;

public sealed record DeliveryAddress
{
    public string Street { get; }
    public string City { get; }
    public double Latitude { get; }
    public double Longitude { get; }
    public string? Reference { get; }

    public DeliveryAddress(
        string street,
        string city,
        double latitude,
        double longitude,
        string? reference = null)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new DomainException("Street is required");
        if (string.IsNullOrWhiteSpace(city))
            throw new DomainException("City is required");
        if (latitude is < -90 or > 90)
            throw new DomainException("Latitude out of range");
        if (longitude is < -180 or > 180)
            throw new DomainException("Longitude out of range");

        Street = street;
        City = city;
        Latitude = latitude;
        Longitude = longitude;
        Reference = reference;
    }
}