# ADR-002: Diseño del dominio del módulo Entregas

| Campo | Valor |
|---|---|
| Estado | Aceptada |
| Fecha | 2026-10-14 |
| Decisores | Autor del proyecto |
| Relacionado con | ADR-001, Actividad 3, `06-casos-de-uso-puertos-adaptadores.md` |

## Contexto

El ADR-001 definió un monolito modular con seis módulos y reglas de
aislamiento por dominio. La Actividad 3 pide modelar un flujo de negocio
acotado y proteger sus invariantes antes de construir integraciones.

El flujo elegido es **asignar una entrega a un repartidor**
(`AssignCourierUseCase`), porque concentra las reglas críticas del módulo
Entregas: no reasignar, no asignar sin stock, no saturar rutas, no asignar
repartidores no disponibles y dejar trazabilidad completa.

## Decisión

Se adopta un **diseño de dominio con agregado `Delivery` como raíz**,
siguiendo Clean Architecture dentro del módulo Entregas.

### Reglas de diseño

1. **`Delivery` es el aggregate root.** Toda modificación de estado pasa por
   sus métodos. Nadie modifica `Status` ni `CourierId` desde fuera.
2. **La entidad protege las invariantes locales.** INV-01 (no doble
   asignación) e INV-06 (no reasignar terminales) viven dentro de
   `Delivery.AssignCourier()`.
3. **El caso de uso protege las invariantes que cruzan contextos.** INV-02
   (stock reservado), INV-03 (capacidad de ruta) e INV-04 (disponibilidad)
   se validan en `AssignCourierUseCase` mediante puertos.
4. **Los puertos los define la Aplicación.** `IDeliveryRepository`,
   `ICourierAvailabilityPort`, `IRouteCapacityPort`,
   `IStockReservationPort` e `IEventPublisher` son interfaces sin
   dependencias de infraestructura.
5. **La infraestructura implementa los puertos.** Repositorios, adaptadores
   HTTP y publicadores de eventos viven fuera del dominio y se conectan por
   inyección de dependencias.
6. **La comunicación entre módulos es por eventos de dominio.** Entregas
   publica `DeliveryAssigned`; Notificaciones e Incidencias lo consumen. No
   hay llamadas directas entre módulos.

## Alternativas consideradas

| Alternativa | Resultado | Motivo principal |
|---|---|---|
| A. Agregado `Delivery` con puertos definidos por Aplicación | **Elegida** | Protege invariantes, desacopla infraestructura y respeta ADR-001 |
| B. Servicio anémico + lógica en el caso de uso | Descartada | Las invariantes quedan dispersas; el estado puede quedar inconsistente |
| C. Entidad `Delivery` que consulta directamente la BD y APIs | Descartada | Acopla el dominio a infraestructura; viola inversión de dependencias |

## Consecuencias

**Positivas**

- El dominio se prueba sin base de datos ni APIs externas.
- Las invariantes están en un solo lugar y son fáciles de auditar.
- Los módulos evolucionan de forma independiente (RNF-09, QS-08).
- La comunicación por eventos respeta el aislamiento del ADR-001.

**Negativas / trade-offs aceptados**

- Más interfaces que mantener.
- Requiere disciplina para no filtrar infraestructura al dominio.
- La verificación de stock cruza contextos, así que no puede ser una
  invariante puramente local; se resuelve como precondición del caso de uso.

## Riesgos y mitigaciones

| Riesgo | Mitigación |
|---|---|
| El dominio se acopla a infraestructura con el tiempo | Pruebas unitarias sin mocks de BD; revisión de dependencias en cada entrega |
| La verificación de stock se vuelve un cuello de botella | Cachear la reserva y usar eventos `StockReserved` en lugar de consultas síncronas si el volumen lo exige |
| Los eventos se pierden o se duplican | Idempotencia en consumidores (RNF-08, QS-09) y cola con reintentos |
| La entidad crece con demasiadas responsabilidades | Revisar límites del agregado en cada iteración; separar `Route` si hace falta |

## Verificación

| Criterio | Cómo se comprueba |
|---|---|
| El dominio no depende de infraestructura | Revisión de `using` en `/src/Domain` |
| Las invariantes se protegen | Tests unitarios de `DeliveryTests.cs` |
| Las precondiciones se respetan | Tests de `AssignCourierUseCaseTests.cs` con puertos falsos |
| Los escenarios se cumplen | QS-ASG-01 a QS-ASG-07 ejecutados como pruebas |
| La dirección de dependencias es correcta | Revisión del diagrama de capas y del código |

## Disparadores para reevaluar la decisión

1. La entidad `Delivery` acumula responsabilidades que no le corresponden.
2. Aparece la necesidad de separar `Route` como agregado independiente.
3. La verificación de stock deja de ser síncrona y pasa a ser un flujo de
   eventos con compensación.
4. El módulo Entregas se extrae como servicio (disparador del ADR-001).