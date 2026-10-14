namespace LastMile.Domain.Entregas;

public sealed class Delivery
{
    public DeliveryId Id { get; }
    public OrderId OrderId { get; }
    public DeliveryAddress Address { get; }
    public DeliveryWindow Window { get; }
    public DeliveryStatus Status { get; private set; }
    public CourierId? CourierId { get; private set; }
    public DateTime? AssignedAt { get; private set; }

    public Delivery(
        DeliveryId id,
        OrderId orderId,
        DeliveryAddress address,
        DeliveryWindow window)
    {
        Id = id ?? throw new DomainException("DeliveryId is required");
        OrderId = orderId ?? throw new DomainException("OrderId is required");
        Address = address ?? throw new DomainException("Address is required");
        Window = window ?? throw new DomainException("Window is required");
        Status = DeliveryStatus.Pendiente;
    }

    /// <summary>
    /// Prepara la entrega para asignación. Transición Pendiente -> Preparada.
    /// </summary>
    public void MarkAsPrepared()
    {
        if (Status != DeliveryStatus.Pendiente)
            throw new DomainException(
                $"Cannot prepare from state {Status}");

        Status = DeliveryStatus.Preparada;
    }

    /// <summary>
    /// Asigna un repartidor a la entrega.
    /// Protege INV-01 (no doble asignación) e INV-06 (no reasignar terminales).
    /// </summary>
    public void AssignCourier(CourierId courierId, string assignedBy)
    {
        // INV-01: no doble asignación
        if (CourierId is not null)
            throw new DomainException("Delivery already assigned");

        // INV-06: no reasignar entregas cerradas
        if (Status is DeliveryStatus.Entregada or DeliveryStatus.Fallida)
            throw new DomainException("Cannot assign a closed delivery");

        // Transición válida: solo desde Preparada
        if (Status != DeliveryStatus.Preparada)
            throw new DomainException(
                $"Cannot assign from state {Status}");

        if (string.IsNullOrWhiteSpace(assignedBy))
            throw new DomainException("AssignedBy is required");

        CourierId = courierId
            ?? throw new DomainException("CourierId is required");
        AssignedAt = DateTime.UtcNow;
        Status = DeliveryStatus.Asignada;
    }

    /// <summary>
    /// Inicia la ruta. Transición Asignada -> EnRuta.
    /// </summary>
    public void StartRoute()
    {
        if (Status != DeliveryStatus.Asignada)
            throw new DomainException(
                $"Cannot start route from state {Status}");

        Status = DeliveryStatus.EnRuta;
    }

    /// <summary>
    /// Confirma la entrega al cliente. Transición EnRuta -> Entregada.
    /// </summary>
    public void ConfirmDelivery()
    {
        if (Status != DeliveryStatus.EnRuta)
            throw new DomainException(
                $"Cannot confirm from state {Status}");

        Status = DeliveryStatus.Entregada;
    }

    /// <summary>
    /// Reporta una incidencia. Transición EnRuta -> ConIncidencia.
    /// </summary>
    public void ReportIncident()
    {
        if (Status != DeliveryStatus.EnRuta)
            throw new DomainException(
                $"Cannot report incident from state {Status}");

        Status = DeliveryStatus.ConIncidencia;
    }

    /// <summary>
    /// Resuelve una incidencia. Transición ConIncidencia -> EnRuta.
    /// </summary>
    public void ResolveIncident()
    {
        if (Status != DeliveryStatus.ConIncidencia)
            throw new DomainException(
                $"Cannot resolve incident from state {Status}");

        Status = DeliveryStatus.EnRuta;
    }

    /// <summary>
    /// Marca como fallida. Solo desde ConIncidencia.
    /// </summary>
    public void MarkAsFailed()
    {
        if (Status != DeliveryStatus.ConIncidencia)
            throw new DomainException(
                $"Cannot fail from state {Status}");

        Status = DeliveryStatus.Fallida;
    }
}