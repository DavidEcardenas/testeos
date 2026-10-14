namespace LastMile.Domain.Entregas;

public enum DeliveryStatus
{
    Pendiente = 0,
    Preparada = 1,
    Asignada = 2,
    EnRuta = 3,
    Entregada = 4,
    ConIncidencia = 5,
    Fallida = 6
}