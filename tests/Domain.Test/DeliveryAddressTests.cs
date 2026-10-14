using LastMile.Domain.Entregas;
using Xunit;

namespace LastMile.Domain.Tests;

public class DeliveryAddressTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesAddress()
    {
        var address = new DeliveryAddress(
            "Calle 100 #15-20", "Bogotá", 4.68, -74.05);

        Assert.Equal("Calle 100 #15-20", address.Street);
        Assert.Equal("Bogotá", address.City);
        Assert.Equal(4.68, address.Latitude);
        Assert.Equal(-74.05, address.Longitude);
    }

    [Fact]
    public void Constructor_WithEmptyStreet_Throws()
    {
        var ex = Assert.Throws<DomainException>(() =>
            new DeliveryAddress("", "Bogotá", 4.68, -74.05));

        Assert.Equal("Street is required", ex.Message);
    }

    [Fact]
    public void Constructor_WithEmptyCity_Throws()
    {
        var ex = Assert.Throws<DomainException>(() =>
            new DeliveryAddress("Calle 100", "", 4.68, -74.05));

        Assert.Equal("City is required", ex.Message);
    }

    [Fact]
    public void Constructor_WithInvalidLatitude_Throws()
    {
        var ex = Assert.Throws<DomainException>(() =>
            new DeliveryAddress("Calle 100", "Bogotá", 100, -74.05));

        Assert.Equal("Latitude out of range", ex.Message);
    }

    [Fact]
    public void Constructor_WithInvalidLongitude_Throws()
    {
        var ex = Assert.Throws<DomainException>(() =>
            new DeliveryAddress("Calle 100", "Bogotá", 4.68, -200));

        Assert.Equal("Longitude out of range", ex.Message);
    }
}