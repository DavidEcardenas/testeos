# Flujo de negocio: Asignar una entrega a un repartidor

## Actores

| Actor | Rol en el flujo |
|---|---|
| Administrador de almacén | Dispara la preparación y solicita la asignación |
| Sistema LastMile | Orquesta la asignación, valida invariantes y publica eventos |
| Repartidor | Recibe la asignación y ejecuta la entrega |
| Agente de soporte | Observa el resultado y gestiona incidencias posteriores |

## Propósito

Garantizar que una entrega preparada sea asignada a un repartidor disponible,
con ruta calculada y dentro de su capacidad, respetando las invariantes del
negocio y dejando trazabilidad completa del cambio de estado.

Este flujo es el caso de uso que la Actividad 3 pide modelar
(`AssignCourierUseCase`) y concentra las reglas críticas del módulo Entregas
del monolito modular definido en el ADR-001.

## Resultado esperado

- La entrega pasa al estado `Asignada`.
- Queda registrado el repartidor activo y la marca de tiempo.
- Se publica el evento `DeliveryAssigned` para Notificaciones e Incidencias.
- Todo cambio queda auditado con fecha, hora y responsable (RNF-06).

## Origen en actividades previas

| Elemento | Actividad 1 | Actividad 2 |
|---|---|---|
| Objetivo | Objetivo 3: automatizar asignación y seguimiento de rutas | — |
| Requisitos | RF-08, RF-09 | RF-08, RF-09 |
| Atributos de calidad | Trazabilidad, consistencia | RNF-03, RNF-06, RNF-09 |
| Decisión estructural | — | ADR-001: monolito modular, módulo Entregas |
| Escenarios de calidad | — | QS-08 (evolución), QS-10 (trazabilidad) |

## Fuera de alcance de este flujo

- Cálculo real de la ruta óptima (se delega al servicio de geolocalización).
- Procesamiento de pagos (RES-01).
- Optimización de flota de larga distancia (RES-03).
- Implementación de persistencia e integraciones reales (Actividad 4).