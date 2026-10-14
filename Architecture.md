# Architecture.md — LastMile

Documento vivo de la arquitectura del sistema operativo logístico LastMile.
Recoge el análisis de la Actividad 1, las decisiones estructurales de la
Actividad 2 y el diseño de dominio de la Actividad 3.

## 1. Contexto

LastMile centraliza pedidos multicanal, consolida inventario distribuido y
automatiza la entrega de última milla. El problema central es la falta de una
fuente única de verdad, que causa ventas sin stock real, entregas fallidas y
sobrecarga de soporte.

## 2. Decisiones estructurales

- **ADR-001:** monolito modular con seis módulos (Pedidos, Inventario,
  Entregas, Incidencias, Notificaciones, Integraciones). Ver
  `docs/actividad-2/05-adr-001-estructura-del-sistema.md`.
- **ADR-002:** diseño del dominio del módulo Entregas con `Delivery` como
  aggregate root. Ver `docs/actividad-3/08-adr-002-dominio-entregas.md`.

## 3. Estructura de capas por módulo

Cada módulo del monolito sigue Clean Architecture:

| Capa | Contenido | Depende de |
|---|---|---|
| **Domain** | Entidades, objetos de valor, invariantes | Nada externo |
| **Application** | Casos de uso, DTOs, puertos | Domain |
| **Infrastructure** | Repositorios, adaptadores externos | Application + Domain |

Regla: las dependencias apuntan siempre hacia el dominio.

## 4. Flujo modelado en la Actividad 3

**Asignar una entrega a un repartidor** (`AssignCourierUseCase`).

- **Aggregate root:** `Delivery`.
- **Puertos:** `IDeliveryRepository`, `ICourierAvailabilityPort`,
  `IRouteCapacityPort`, `IStockReservationPort`, `IEventPublisher`.
- **Invariantes:** INV-01 a INV-07 (ver `docs/actividad-3/04-invariantes-y-escenarios.md`).
- **Escenarios de aceptación:** QS-ASG-01 a QS-ASG-07.

Documentación completa en `docs/actividad-3/06-casos-de-uso/`.

## 5. Estructura del repositorio

```
/docs
  /actividad-1    Análisis del problema
  /actividad-2    Catálogo, escenarios, comparación, ADR-001, criterios
  /actividad-3    Flujo, lenguaje, modelo, invariantes, contextos, casos de uso, C4, ADR-002
/src
  /Domain
    /Entregas     Delivery, IDs, VOs, DomainException
  /Application
    /Entregas     AssignCourierUseCase, Command, Result, Evento, /Ports
  /Infrastructure (Actividad 4)
/tests
  /Domain.Tests       DeliveryTests, DeliveryAddressTests, DeliveryWindowTests
  /Application.Tests  AssignCourierUseCaseTests + Fakes
```

## 6. Diagramas C4

Los diagramas C4 (niveles 1, 2 y 3) están en
`docs/actividad-3/07-c4-actualizado.md`.

## 7. Bounded contexts

Los contextos delimitados y el Context Map están en
`docs/actividad-3/05-bounded-contexts.md`.

## 8. Requisitos y verificación

- Catálogo de requisitos: `docs/actividad-2/01-catalogo-de-requisitos.md`.
- Escenarios de calidad: `docs/actividad-2/03-escenarios-de-calidad.md`.
- Criterios de verificación de la decisión:
  `docs/actividad-2/06-criterios-de-verificacion.md`.
- Escenarios de aceptación del dominio:
  `docs/actividad-3/04-invariantes-y-escenarios.md`.

## 9. Estado y próximos pasos

- **Actividad 1:** completada.
- **Actividad 2:** completada.
- **Actividad 3:** completada (diseño de dominio + tests + documentación).
- **Actividad 4:** pendiente. Implementar persistencia, adaptadores externos
  y flujo end-to-end.

## 10. Restricciones vigentes

- Cobranza delegada a pasarelas de pago (RES-01).
- Facturación contable delegada al ERP (RES-02).
- No se optimiza flota de larga distancia (RES-03).
- El ERP y el catálogo/e-commerce ya existen (RES-04).
- Los repartidores usan dispositivos móviles con datos y GPS (RES-05).
- Integración mediante REST/webhooks/eventos (RES-06).
- Inventario centralizado con sincronización periódica (RES-07).
- Normativa de protección de datos por confirmar (RES-08).