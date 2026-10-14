```markdown
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