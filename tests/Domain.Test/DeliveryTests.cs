using LastMile.Domain.Entregas;
using Xunit;

namespace LastMile.Domain.Tests;

public class DeliveryTests
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

    // QS-ASG-01
    [Fact]
    public void AssignCourier_FromPrepared_ChangesStatusToAsignada()
    {
        var delivery = CreatePreparedDelivery();
        var courierId = CourierId.New();

        delivery.AssignCourier(courierId, "admin@lastmile.com");

        Assert.Equal(DeliveryStatus.Asignada, delivery.Status);
        Assert.Equal(courierId, delivery.CourierId);
        Assert.NotNull(delivery.AssignedAt);
    }

    // QS-ASG-02 (INV-01)
    [Fact]
    public void AssignCourier_WhenAlreadyAssigned_Throws()
    {
        var delivery = CreatePreparedDelivery();
        delivery.AssignCourier(CourierId.New(), "admin@lastmile.com");

        var ex = Assert.Throws<DomainException>(() =>
            delivery.AssignCourier(CourierId.New(), "admin@lastmile.com"));

        Assert.Equal("Delivery already assigned", ex.Message);
    }

    // QS-ASG-07 (INV-06)
    [Fact]
    public void AssignCourier_WhenDelivered_Throws()
    {
        var delivery = CreatePreparedDelivery();
        delivery.AssignCourier(CourierId.New(), "admin@lastmile.com");
        delivery.StartRoute();
        delivery.ConfirmDelivery();

        var ex = Assert.Throws<DomainException>(() =>
            delivery.AssignCourier(CourierId.New(), "admin@lastmile.com"));

        Assert.Equal("Cannot assign a closed delivery", ex.Message);
    }

    // Transición inválida
    [Fact]
    public void AssignCourier_FromPendiente_Throws()
    {
        var delivery = new Delivery(
            DeliveryId.New(),
            OrderId.New(),
            new DeliveryAddress("Calle 100 #15-20", "Bogotá", 4.68, -74.05),
            new DeliveryWindow(
                DateTime.UtcNow.AddHours(1),
                DateTime.UtcNow.AddHours(3)));

        var ex = Assert.Throws<DomainException>(() =>
            delivery.AssignCourier(CourierId.New(), "admin@lastmile.com"));

        Assert.Equal("Cannot assign from state Pendiente", ex.Message);
    }

    [Fact]
    public void MarkAsPrepared_FromPendiente_ChangesStatus()
    {
        var delivery = new Delivery(
            DeliveryId.New(),
            OrderId.New(),
            new DeliveryAddress("Calle 100 #15-20", "Bogotá", 4.68, -74.05),
            new DeliveryWindow(
                DateTime.UtcNow.AddHours(1),
                DateTime.UtcNow.AddHours(3)));

        delivery.MarkAsPrepared();

        Assert.Equal(DeliveryStatus.Preparada, delivery.Status);
    }

    [Fact]
    public void MarkAsPrepared_WhenAlreadyPrepared_Throws()
    {
        var delivery = CreatePreparedDelivery();

        Assert.Throws<DomainException>(() => delivery.MarkAsPrepared());
    }

    [Fact]
    public void StartRoute_FromAsignada_ChangesStatus()
    {
        var delivery = CreatePreparedDelivery();
        delivery.AssignCourier(CourierId.New(), "admin@lastmile.com");

        delivery.StartRoute();

        Assert.Equal(DeliveryStatus.EnRuta, delivery.Status);
    }

    [Fact]
    public void ConfirmDelivery_FromEnRuta_ChangesStatus()
    {
        var delivery = CreatePreparedDelivery();
        delivery.AssignCourier(CourierId.New(), "admin@lastmile.com");
        delivery.StartRoute();

        delivery.ConfirmDelivery();

        Assert.Equal(DeliveryStatus.Entregada, delivery.Status);
    }

    [Fact]
    public void ReportIncident_FromEnRuta_ChangesStatus()
    {
        var delivery = CreatePreparedDelivery();
        delivery.AssignCourier(CourierId.New(), "admin@lastmile.com");
        delivery.StartRoute();

        delivery.ReportIncident();

        Assert.Equal(DeliveryStatus.ConIncidencia, delivery.Status);
    }

    [Fact]
    public void ResolveIncident_FromConIncidencia_ReturnsToEnRuta()
    {
        var delivery = CreatePreparedDelivery();
        delivery.AssignCourier(CourierId.New(), "admin@lastmile.com");
        delivery.StartRoute();
        delivery.ReportIncident();

        delivery.ResolveIncident();

        Assert.Equal(DeliveryStatus.EnRuta, delivery.Status);
    }

    [Fact]
    public void MarkAsFailed_FromConIncidencia_ChangesStatus()
    {
        var delivery = CreatePreparedDelivery();
        delivery.AssignCourier(CourierId.New(), "admin@lastmile.com");
        delivery.StartRoute();
        delivery.ReportIncident();

        delivery.MarkAsFailed();

        Assert.Equal(DeliveryStatus.Fallida, delivery.Status);
    }

    [Fact]
    public void AssignCourier_WithEmptyAssignedBy_Throws()
    {
        var delivery = CreatePreparedDelivery();

        var ex = Assert.Throws<DomainException>(() =>
            delivery.AssignCourier(CourierId.New(), ""));

        Assert.Equal("AssignedBy is required", ex.Message);
    }
}