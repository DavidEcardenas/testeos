# 06 — Casos de uso, puertos y adaptadores

Este bloque documenta el diseño del caso de uso principal del módulo
Entregas: `AssignCourierUseCase`. Está dividido en archivos pequeños para
facilitar su lectura y mantenimiento.

## Índice

| # | Documento | Contenido |
|---|---|---|
| 06.1 | [Contrato de aplicación](./06.1-contrato-de-aplicacion.md) | Command y Result del caso de uso |
| 06.2 | [Flujo del caso de uso](./06.2-flujo-del-caso-de-uso.md) | Diagrama de secuencia |
| 06.3 | [Implementación del caso de uso](./06.3-implementacion-del-caso-de-uso.md) | Código de `AssignCourierUseCase` |
| 06.4 | [Puertos](./06.4-puertos.md) | Interfaces que la Aplicación define |
| 06.5 | [Contratos de puertos](./06.5-contratos-de-puertos.md) | Firma de cada interfaz |
| 06.6 | [Adaptadores](./06.6-adaptadores.md) | Implementaciones previstas para Actividad 4 |
| 06.7 | [Dirección de dependencias](./06.7-direccion-de-dependencias.md) | Diagrama de capas |
| 06.8 | [Otros casos de uso](./06.8-otros-casos-de-uso.md) | StartDelivery, ConfirmDelivery, ReportIncident |

## Caso de uso principal

`AssignCourierUseCase` asigna un repartidor disponible a una entrega
preparada, respetando las invariantes del negocio y publicando el resultado
para que otros módulos reaccionen.