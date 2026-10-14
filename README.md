# LastMile: sistema operativo logístico

## Resumen del problema

La operación actual funciona en silos: cada canal de venta maneja su propio stock y la coordinación entre almacén, repartidor y soporte es manual. Sin una fuente única de verdad no se conoce el estado real de un paquete y se vende sin stock disponible, lo que genera entregas fallidas, clientes sin información y soporte sobrecargado.

LastMile centraliza los estados de pedidos de múltiples canales, da visibilidad del inventario consolidado y automatiza la asignación y seguimiento de las rutas de última milla. Su meta es reducir un 35% los errores operativos por duplicación e inconsistencia de datos.



## Requisitos prioritarios

Catálogo completo: 14 requisitos funcionales y 10 atributos de calidad. Los que más condicionan la arquitectura son:

| Prioridad | Requisito | Meta verificable |
|---|---|---|
| 1 | Consistencia de inventario y reserva de stock (RNF-03, RF-06) | 0 sobreventas ante reservas simultáneas |
| 2 | Estado único del pedido y trazabilidad (RF-02, RNF-06) | Consulta p95 < 2 s; 100 % de cambios auditados |
| 3 | Interoperabilidad con ERP/CRM, pasarela y servicios externos (RNF-08) | 0 pedidos duplicados ante reenvíos |
| 4 | Disponibilidad y capacidad ante picos (RNF-04, RNF-02) | Una falla externa no detiene pedidos; 3 veces la carga normal sin perder pedidos |
| 5 | Evolución por módulos (RNF-09) | Un cambio de regla afecta a un solo módulo |
| 6 | Seguridad de datos personales (RNF-05) | Cifrado y acceso por rol |



## Decisión arquitectónica

**Monolito modular con límites de dominio claros**, desplegado como una sola unidad, con los módulos Pedidos, Inventario, Entregas, Incidencias, Notificaciones e Integraciones. Cada módulo es dueño de sus datos y se comunica con los demás solo por interfaces o eventos internos.

| Alternativa | Puntaje ponderado | Resultado |
|---|---|---|
| A. Monolito modular | 4,35 | **Elegida** |
| B. Servicios distribuidos por dominio | 2,75 | Descartada por ahora |
| C. Monolito por capas | 3,95 | Descartada |

**Por qué:** la reserva de stock puede resolverse con una transacción local, la operación es más simple y barata, y los límites por dominio permiten extraer servicios más adelante si el volumen lo justifica. Detalle y trade-offs.

La Actividad 3 continúa esa decisión con un **ADR-002** que fija el diseño del dominio del módulo Entregas: `Delivery` como aggregate root, invariantes protegidas por la entidad y los casos de uso, y puertos definidos por la Aplicación. Ver [ADR-002](./docs/actividad-3/08-adr-002-dominio-entregas.md).



## Documentación

### Comprender el problema

| Documento | Contenido |
|---|---|
| [Análisis del problema](./docs/actividad-1/01-analisis-del-problema.md) | Problema, usuarios, objetivos, alcance y consecuencias |
| [Supuestos](./docs/actividad-1/02-supuestos.md) | Supuestos, incógnitas y puntos por confirmar |
| [Diagrama de contexto](./docs/actividad-1/03-diagrama-de-contexto.md) | Contexto y flujos de información |
| [Decisiones y riesgos](./docs/actividad-1/04-decisiones-y-riesgos.md) | Decisiones iniciales y riesgos |

### Requisitos y decisión estructural

| Documento | Contenido |
|---|---|
| [Catálogo de requisitos](./docs/actividad-2/01-catalogo-de-requisitos.md) | Requisitos funcionales, atributos de calidad y restricciones |
| [Matriz de priorización](./docs/actividad-2/02-matriz-de-priorizacion.md) | Prioridad, impacto y complejidad; drivers arquitectónicos |
| [Escenarios de calidad](./docs/actividad-2/03-escenarios-de-calidad.md) | 10 escenarios con medidas concretas |
| [Comparación de alternativas](./docs/actividad-2/04-comparacion-de-alternativas.md) | Tres alternativas con criterios ponderados |
| [ADR-001](./docs/actividad-2/05-adr-001-estructura-del-sistema.md) | Decisión, alternativas, justificación, riesgos y trade-offs |
| [Criterios de verificación](./docs/actividad-2/06-criterios-de-verificacion.md) | Criterios verificables de la decisión |
| [Guion del video](./docs/actividad-2/07-guion-video.md) | Apoyo para la sustentación |

### Diseño y dominio

| Documento | Contenido |
|---|---|
| [Flujo de negocio](./docs/actividad-3/01-flujo-de-negocio.md) | Caso de uso modelado: asignar entrega a repartidor |
| [Lenguaje ubicuo](./docs/actividad-3/02-lenguaje-ubicuo.md) | Términos consensuados y términos ambiguos por confirmar |
| [Modelo de dominio](./docs/actividad-3/03-modelo-de-dominio.md) | Entidades, objetos de valor y estados de `Delivery` |
| [Invariantes y escenarios](./docs/actividad-3/04-invariantes-y-escenarios.md) | INV-01 a INV-07 y escenarios QS-ASG-01 a QS-ASG-07 |
| [Bounded contexts](./docs/actividad-3/05-bounded-contexts.md) | Contextos delimitados y Context Map |
| [Casos de uso, puertos y adaptadores](./docs/actividad-3/06-casos-de-uso/README.md) | `AssignCourierUseCase`, puertos y dirección de dependencias |
| [C4 actualizado](./docs/actividad-3/07-c4-actualizado.md) | Niveles 1, 2 y 3 con el módulo Entregas |
| [ADR-002](./docs/actividad-3/08-adr-002-dominio-entregas.md) | Decisión de diseño del dominio del módulo Entregas |



## Estructura del repositorio

```text
.
├── README.md
├── Architecture.md
├── src/
│   ├── Domain/
│   │   └── Entregas/
│   └── Application/
│       └── Entregas/
├── tests/
│   ├── Domain.Tests/
│   └── Application.Tests/
└── docs/
    ├── actividad-1/
    │   ├── 01-analisis-del-problema.md
    │   ├── 02-supuestos.md
    │   ├── 03-diagrama-de-contexto.md
    │   └── 04-decisiones-y-riesgos.md
    ├── actividad-2/
    │   ├── 01-catalogo-de-requisitos.md
    │   ├── 02-matriz-de-priorizacion.md
    │   ├── 03-escenarios-de-calidad.md
    │   ├── 04-comparacion-de-alternativas.md
    │   ├── 05-adr-001-estructura-del-sistema.md
    │   └── 06-criterios-de-verificacion.md
    └── actividad-3/
        ├── 01-flujo-de-negocio.md
        ├── 02-lenguaje-ubicuo.md
        ├── 03-modelo-de-dominio.md
        ├── 04-invariantes-y-escenarios.md
        ├── 05-bounded-contexts.md
        ├── 06-casos-de-uso/
        │   ├── README.md
        │   ├── 06.1-contrato-de-aplicacion.md
        │   ├── 06.2-flujo-del-caso-de-uso.md
        │   ├── 06.3-implementacion-del-caso-de-uso.md
        │   ├── 06.4-puertos.md
        │   ├── 06.5-contratos-de-puertos.md
        │   ├── 06.6-adaptadores.md
        │   ├── 06.7-direccion-de-dependencias.md
        │   └── 06.8-otros-casos-de-uso.md
        ├── 07-c4-actualizado.md
        └── 08-adr-002-dominio-entregas.md
```



## Código

El diseño de dominio de la Actividad 3 se materializa en las carpetas `src/` y `tests/`, siguiendo Clean Architecture dentro del módulo Entregas.

### Estructura de código

```text
src/
├── Domain/
│   └── Entregas/
│       ├── Delivery.cs
│       ├── DeliveryId.cs
│       ├── CourierId.cs
│       ├── OrderId.cs
│       ├── DeliveryStatus.cs
│       ├── DeliveryAddress.cs
│       ├── DeliveryWindow.cs
│       └── DomainException.cs
└── Application/
    └── Entregas/
        ├── AssignCourierUseCase.cs
        ├── AssignCourierCommand.cs
        ├── AssignCourierResult.cs
        ├── DeliveryAssigned.cs
        └── Ports/
            ├── IDeliveryRepository.cs
            ├── ICourierAvailabilityPort.cs
            ├── IRouteCapacityPort.cs
            ├── IStockReservationPort.cs
            └── IEventPublisher.cs

tests/
├── Domain.Tests/
│   ├── DeliveryTests.cs
│   ├── DeliveryAddressTests.cs
│   └── DeliveryWindowTests.cs
└── Application.Tests/
    ├── AssignCourierUseCaseTests.cs
    └── Fakes/
        ├── FakeDeliveryRepository.cs
        ├── FakeCourierAvailabilityPort.cs
        ├── FakeRouteCapacityPort.cs
        ├── FakeStockReservationPort.cs
        └── FakeEventPublisher.cs
```

### Cómo correr los tests

```bash
dotnet test
```

### Invariantes cubiertas por los tests

| Invariante | Test |
|---|---|
| INV-01 (no doble asignación) | `AssignCourier_WhenAlreadyAssigned_Throws` |
| INV-02 (stock reservado) | `Execute_WhenStockNotReserved_Throws` |
| INV-03 (capacidad de ruta) | `Execute_WhenRouteHasNoCapacity_Throws` |
| INV-04 (disponibilidad) | `Execute_WhenCourierNotAvailable_Throws` |
| INV-06 (no reasignar terminales) | `AssignCourier_WhenDelivered_Throws` |


