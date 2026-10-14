using LastMile.Domain.Entregas;
using Xunit;

namespace LastMile.Domain.Tests;

public class DeliveryWindowTests
{
    [Fact]
    public void Constructor_WithValidRange_CreatesWindow()
    {
        var start = DateTime.UtcNow.AddHours(1);
        var end = DateTime.UtcNow.AddHours(3);

        var window = new DeliveryWindow(start, end);

        Assert.Equal(start, window.Start);
        Assert.Equal(end, window.End);
    }

    [Fact]
    public void Constructor_WithEndBeforeStart_Throws()
    {
        var start = DateTime.UtcNow.AddHours(3);
        var end = DateTime.UtcNow.AddHours(1);

        var ex = Assert.Throws<DomainException>(() =>
            new DeliveryWindow(start, end));

        Assert.Equal("Window end must be after start", ex.Message);
    }

    [Fact]
    public void Constructor_WithEqualStartAndEnd_Throws()
    {
        var now = DateTime.UtcNow;

        Assert.Throws<DomainException>(() =>
            new DeliveryWindow(now, now));
    }

    [Fact]
    public void IsInThePast_WhenEndBeforeNow_ReturnsTrue()
    {
        var window = new DeliveryWindow(
            DateTime.UtcNow.AddHours(-3),
            DateTime.UtcNow.AddHours(-1));

        Assert.True(window.IsInThePast(DateTime.UtcNow));
    }

    [Fact]
    public void IsInThePast_WhenEndAfterNow_ReturnsFalse()
    {
        var window = new DeliveryWindow(
            DateTime.UtcNow.AddHours(1),
            DateTime.UtcNow.AddHours(3));

        Assert.False(window.IsInThePast(DateTime.UtcNow));
    }
}