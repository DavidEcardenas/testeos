# Modelo de dominio

## Entidades

Objetos con identidad propia. Su identidad no cambia aunque cambien sus atributos.

| Entidad | Identidad | Responsabilidad | Módulo |
|---|---|---|---|
| `Delivery` | `DeliveryId` | Representa la entrega; protege la invariante de asignación única y las transiciones de estado | Entregas |
| `Order` | `OrderId` | Agrupa líneas de pedido; origen del flujo | Pedidos |
| `Courier` | `CourierId` | Repartidor con estado de disponibilidad | Entregas |
| `Route` | `RouteId` | Secuencia de paradas asignadas a un repartidor | Entregas |
| `Incident` | `IncidentId` | Desviación reportada durante la entrega | Incidencias |
| `WarehouseNode` | `NodeId` | Nodo de despacho con inventario | Inventario |

## Objetos de valor

Sin identidad propia; se describen por sus atributos. Son inmutables.

| Objeto de valor | Atributos | Regla asociada |
|---|---|---|
| `DeliveryId` | `Guid Value` | Identidad tipada, no un `Guid` suelto |
| `CourierId` | `Guid Value` | Identidad tipada |
| `OrderId` | `Guid Value` | Identidad tipada |
| `DeliveryAddress` | calle, ciudad, coordenadas, referencia | Debe ser válida y geolocalizable |
| `DeliveryWindow` | inicio, fin | No puede ser anterior a "ahora" al asignar |
| `RouteCapacity` | maxParadas, pesoMax, volumenMax | No puede superarse (INV-03) |
| `CourierAvailability` | estado, franjaHoraria | Solo `Available` permite asignar (INV-04) |
| `IncidentReason` | código, descripción | Debe pertenecer al catálogo |
| `TelemetryPoint` | latitud, longitud, timestamp | Debe ser monótono en el tiempo |

## Agregado `Delivery`

`Delivery` es el **aggregate root** del flujo modelado. Todo acceso pasa por él.

```mermaid
classDiagram
    class Delivery {
        +DeliveryId Id
        +OrderId OrderId
        +DeliveryAddress Address
        +DeliveryWindow Window
        +DeliveryStatus Status
        +CourierId? CourierId
        +DateTime? AssignedAt
        +AssignCourier(CourierId, string)
    }
    class DeliveryId
    class OrderId
    class CourierId
    class DeliveryAddress
    class DeliveryWindow
    class DeliveryStatus {
        <<enumeration>>
        Pendiente
        Preparada
        Asignada
        EnRuta
        Entregada
        ConIncidencia
        Fallida
    }
    Delivery --> DeliveryId
    Delivery --> OrderId
    Delivery --> CourierId
    Delivery --> DeliveryAddress
    Delivery --> DeliveryWindow
    Delivery --> DeliveryStatus