# Invariantes y escenarios de aceptación

## Invariantes

Una invariante es una regla que **siempre** debe cumplirse. Si el sistema
permite violarla, el modelo está mal.

| ID | Invariante | Módulo | Requisito |
|---|---|---|---|
| INV-01 | Una entrega no puede tener dos repartidores activos simultáneamente | Entregas | RF-08, RNF-03 |
| INV-02 | No se puede asignar una entrega si el stock del pedido no está reservado | Inventario / Entregas | RF-06, RNF-03 |
| INV-03 | No se puede asignar si la ruta del repartidor está saturada (`RouteCapacity`) | Entregas | RF-08 |
| INV-04 | Solo un repartidor con `CourierAvailability = Available` puede recibir asignación | Entregas | RF-08 |
| INV-05 | Una incidencia no puede cerrarse sin resolución y responsable | Incidencias | RF-10, RNF-06 |
| INV-06 | Una entrega en estado `Entregada` o `Fallida` es terminal | Entregas | RF-09 |
| INV-07 | Todo cambio de estado de entrega queda auditado con fecha, hora y responsable | Transversal | RNF-06 |

### Dónde se protege cada invariante

| Invariante | Dónde vive | Por qué ahí |
|---|---|---|
| INV-01, INV-06 | Dentro de `Delivery.AssignCourier()` | Es una regla del agregado; no depende de nada externo |
| INV-02 | En `AssignCourierUseCase` (precondición) | Cruza contextos (Entregas ↔ Inventario); no puede vivir en la entidad sin acoplarla |
| INV-03, INV-04 | En `AssignCourierUseCase` (precondición) | Requieren consultar puertos externos al agregado |
| INV-05 | En `Incident.Close()` | Regla propia del agregado Incidencia |
| INV-07 | En el repositorio y el publicador de eventos | Requiere persistencia y auditoría, que son infraestructura |

## Escenarios de aceptación (Dado-Cuando-Entonces)

### QS-ASG-01 — Asignación exitosa

> **Dado** una entrega en estado `Preparada` con stock reservado
> **Y** un repartidor con disponibilidad `Available` y ruta no saturada
> **Cuando** el administrador solicita asignar la entrega al repartidor
> **Entonces** la entrega pasa a estado `Asignada`
> **Y** se registra el `courierId` y la marca de tiempo
> **Y** se publica un evento `DeliveryAssigned`

### QS-ASG-02 — Rechazo por doble asignación (INV-01)

> **Dado** una entrega que ya tiene un repartidor activo
> **Cuando** se intenta asignar un segundo repartidor
> **Entonces** el sistema rechaza la operación con error `Delivery already assigned`
> **Y** la entrega conserva su repartidor original
> **Y** el intento queda auditado

### QS-ASG-03 — Rechazo por stock no reservado (INV-02)

> **Dado** una entrega cuyo pedido no tiene stock reservado
> **Cuando** se intenta asignar un repartidor
> **Entonces** el sistema rechaza la operación con error `Stock not reserved`
> **Y** la entrega permanece en estado `Preparada`

### QS-ASG-04 — Rechazo por ruta saturada (INV-03)

> **Dado** un repartidor cuya ruta alcanzó su `RouteCapacity`
> **Cuando** se intenta asignarle una nueva entrega
> **Entonces** el sistema rechaza la asignación con error `Route capacity exceeded`
> **Y** sugiere repartidores alternativos disponibles

### QS-ASG-05 — Rechazo por repartidor no disponible (INV-04)

> **Dado** un repartidor con disponibilidad `Unavailable`
> **Cuando** se intenta asignarle una entrega
> **Entonces** el sistema rechaza con error `Courier not available`

### QS-ASG-06 — Incidencia sin resolución (INV-05)

> **Dado** una incidencia abierta
> **Cuando** un agente intenta cerrarla sin registrar resolución ni responsable
> **Entonces** el sistema rechaza el cierre con error `Resolution and owner required`

### QS-ASG-07 — No reasignar entrega terminal (INV-06)

> **Dado** una entrega en estado `Entregada`
> **Cuando** se intenta asignarle un repartidor
> **Entonces** el sistema rechaza con error `Cannot assign a closed delivery`

## Trazabilidad a escenarios de calidad de la Actividad 2

| Escenario Act 3 | Se relaciona con |
|---|---|
| QS-ASG-01 | QS-10 (trazabilidad de incidencia) |
| QS-ASG-02 | QS-02 (consistencia, no sobreventa) |
| QS-ASG-03 | QS-02 (consistencia de inventario) |
| QS-ASG-04 | RF-08, RNF-01 |
| QS-ASG-05 | RF-08 |
| QS-ASG-06 | QS-10, RNF-06 |
| QS-ASG-07 | RNF-03, RNF-06 |