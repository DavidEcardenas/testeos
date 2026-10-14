# Lenguaje ubicuo

## Términos consensuados

| Término | Definición | Módulo dueño |
|---|---|---|
| **Entrega (Delivery)** | Unidad logística que representa el traslado de un pedido preparado desde un nodo hasta el cliente final | Entregas |
| **Pedido (Order)** | Solicitud del cliente que agrupa una o más líneas de producto | Pedidos |
| **Repartidor (Courier)** | Persona que ejecuta la entrega en la última milla | Entregas |
| **Ruta (Route)** | Secuencia planificada de paradas asignadas a un repartidor en una jornada | Entregas |
| **Nodo / Almacén (Node)** | Punto físico desde el cual se despacha (almacén central, tienda, micro-hub) | Inventario |
| **Incidencia (Incident)** | Evento que desvía la entrega del flujo normal | Incidencias |
| **Telemetría** | Datos de ubicación y estado enviados por el dispositivo del repartidor | Entregas |
| **Capacidad de ruta** | Límite de trabajo que puede asumir un repartidor | Entregas |
| **Disponibilidad del repartidor** | Estado que indica si puede recibir nuevas asignaciones | Entregas |
| **Reserva de stock** | Compromiso de unidades de un producto para un pedido, sin venderlas a otro canal | Inventario |
| **Fuente única de verdad** | Estado canónico del pedido y del stock, consultable por todos los módulos | Transversal |

## Términos ambiguos a confirmar

| Término | Ambigüedad | Impacto si no se aclara |
|---|---|---|
| **Entrega** | ¿Un pedido = una entrega, o un pedido puede dividirse en varias entregas por falta de stock parcial? | Cambia la cardinalidad Pedido–Entrega y el modelo de datos |
| **Capacidad de ruta** | ¿Se mide en número de paradas, peso, volumen o tiempo estimado? | Cambia la invariante INV-03 y el puerto `IRouteCapacityPort` |
| **Disponibilidad del repartidor** | ¿Se calcula por jornada, por franja horaria o en tiempo real? | Cambia la frecuencia de consulta y el contrato del puerto |
| **Repartidor** | ¿Es empleado propio, tercero o híbrido? | Afecta reglas de asignación y permisos (RF-14) |
| **Tienda física** | ¿Es un nodo con inventario propio o solo un punto de despacho? | Afecta RF-13 y el modelo de Inventario |
| **Incidencia** | ¿Quién puede reportarla: solo el repartidor o también el cliente? | Afecta RF-10 y los permisos por rol |
| **Telemetría** | ¿Con qué frecuencia se envía y cuánto tiempo se retiene? | Afecta RNF-05 (privacidad) y RNF-07 (resiliencia) |

## Reglas de nombrado

- Los conceptos del dominio se nombran igual en código, documentación y conversaciones con negocio.
- En código, los agregados y entidades usan singular (`Delivery`, no `Deliveries`).
- Los eventos de dominio se nombran en pasado (`DeliveryAssigned`, no `AssignDelivery`).
- Los comandos de aplicación se nombran en imperativo (`AssignCourierCommand`).
- Los puertos usan prefijo `I` y describen necesidad, no tecnología (`IDeliveryRepository`, no `ISqlDeliveryRepository`).