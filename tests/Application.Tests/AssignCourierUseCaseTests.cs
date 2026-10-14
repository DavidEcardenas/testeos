using LastMile.Application.Entregas;
using LastMile.Application.Tests.Fakes;
using LastMile.Domain.Entregas;
using Xunit;

namespace LastMile.Application.Tests;

public class AssignCourierUseCaseTests
{
    private static Delivery CreatePreparedDelivery()
    {
        var delivery = new Delivery(
            DeliveryId.New(),
            OrderId.New(),
            new DeliveryAddress("Calle 100 #15-20", "Bogotá", 4.68, -74.05),
            new DeliveryWindow(
                DateTime.UtcNow.AddHours(1),
                DateTime.UtcNow.AddHours(3)));

        delivery.MarkAsPrepared();
        return delivery;
    }

    private static (
        AssignCourierUseCase useCase,
        FakeDeliveryRepository repo,
        FakeCourierAvailabilityPort courier,
        FakeRouteCapacityPort route,
        FakeStockReservationPort stock,
        FakeEventPublisher events) Build()
    {
        var repo = new FakeDeliveryRepository();
        var courier = new FakeCourierAvailabilityPort();
        var route = new FakeRouteCapacityPort();
        var stock = new FakeStockReservationPort();
        var events = new FakeEventPublisher();

        var useCase = new AssignCourierUseCase(
            repo, courier, route, stock, events);

        return (useCase, repo, courier, route, stock, events);
    }

    // QS-ASG-01
    [Fact]
    public async Task Execute_WithValidInput_AssignsCourierAndPublishesEvent()
    {
        var (useCase, repo, _, _, _, events) = Build();
        var delivery = CreatePreparedDelivery();
        repo.Seed(delivery);
        var courierId = CourierId.New();

        var result = await useCase.ExecuteAsync(
            new AssignCourierCommand(
                delivery.Id.Value, courierId.Value, "admin@lastmile.com"),
            CancellationToken.None);

        Assert.Equal(delivery.Id.Value, result.DeliveryId);
        Assert.Equal(courierId.Value, result.CourierId);
        Assert.Single(events.Published);
        Assert.IsType<DeliveryAssigned>(events.Published[0]);
    }

    // QS-ASG-02 (INV-01)
    [Fact]
    public async Task Execute_WhenDeliveryAlreadyAssigned_Throws()
    {
        var (useCase, repo, _, _, _, _) = Build();
        var delivery = CreatePreparedDelivery();
        delivery.AssignCourier(CourierId.New(), "admin@lastmile.com");
        repo.Seed(delivery);

        var ex = await Assert.ThrowsAsync<DomainException>(() =>
            useCase.ExecuteAsync(
                new AssignCourierCommand(
                    delivery.Id.Value,
                    CourierId.New().Value,
                    "admin@lastmile.com"),
                CancellationToken.None));

        Assert.Equal("Delivery already assigned", ex.Message);
    }

    // QS-ASG-03 (INV-02)
    [Fact]
    public async Task Execute_WhenStockNotReserved_Throws()
    {
        var (useCase, repo, _, _, stock, _) = Build();
        var delivery = CreatePreparedDelivery();
        repo.Seed(delivery);
        stock.Reserved = false;

        var ex = await Assert.ThrowsAsync<DomainException>(() =>
            useCase.ExecuteAsync(
                new AssignCourierCommand(
                    delivery.Id.Value,
                    CourierId.New().Value,
                    "admin@lastmile.com"),
                CancellationToken.None));

        Assert.Equal("Stock not reserved", ex.Message);
    }

    // QS-ASG-05 (INV-04)
    [Fact]
    public async Task Execute_WhenCourierNotAvailable_Throws()
    {
        var (useCase, repo, courier, _, _, _) = Build();
        var delivery = CreatePreparedDelivery();
        repo.Seed(delivery);
        courier.Available = false;

        var ex = await Assert.ThrowsAsync<DomainException>(() =>
            useCase.ExecuteAsync(
                new AssignCourierCommand(
                    delivery.Id.Value,
                    CourierId.New().Value,
                    "admin@lastmile.com"),
                CancellationToken.None));

        Assert.Equal("Courier not available", ex.Message);
    }

    // QS-ASG-04 (INV-03)
    [Fact]
    public async Task Execute_WhenRouteHasNoCapacity_Throws()
    {
        var (useCase, repo, _, route, _, _) = Build();
        var delivery = CreatePreparedDelivery();
        repo.Seed(delivery);
        route.HasCapacity = false;

        var ex = await Assert.ThrowsAsync<DomainException>(() =>
            useCase.ExecuteAsync(
                new AssignCourierCommand(
                    delivery.Id.Value,
                    CourierId.New().Value,
                    "admin@lastmile.com"),
                CancellationToken.None));

        Assert.Equal("Route capacity exceeded", ex.Message);
    }

    [Fact]
    public async Task Execute_WhenDeliveryNotFound_Throws()
    {
        var (useCase, _, _, _, _, _) = Build();

        var ex = await Assert.ThrowsAsync<DomainException>(() =>
            useCase.ExecuteAsync(
                new AssignCourierCommand(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "admin@lastmile.com"),
                CancellationToken.None));

        Assert.Equal("Delivery not found", ex.Message);
    }
}