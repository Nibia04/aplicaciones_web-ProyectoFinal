# Contexto del proyecto

## Estado actual

El proyecto fue migrado a una solucion **.NET 10 + Aspire** para cumplir la rubrica final.

La version evaluable esta en:

```text
Presupuesto.slnx
src/
  Presupuesto.Api/
  Presupuesto.Application/
  Presupuesto.Domain/
  Presupuesto.Infrastructure/
  Presupuesto.AppHost/
  Presupuesto.ServiceDefaults/
tests/
  Presupuesto.Tests/
```

La entrega principal es 100% .NET.

## Funcionalidades mantenidas

- Registrar usuarios.
- Iniciar sesion con email y contrasena.
- Generar JWT.
- Crear transacciones de ingreso o gasto.
- Listar transacciones del usuario autenticado.
- Consultar transaccion por id.
- Actualizar transaccion.
- Eliminar transaccion.
- Consultar resumen de presupuesto.
- Consumir flujos desde archivo `.http`.

## Cumplimiento de rubrica

### DDD

Implementado en `src/Presupuesto.Domain`:

- Entidades: `Usuario`, `Transaccion`.
- Value Objects: `Email`, `Nombre`, `Dinero`, `Descripcion`, `Categoria`.
- Eventos de dominio: `UsuarioRegistradoEventoDominio`, `TransaccionCreadaEventoDominio`.
- Encapsulamiento con setters privados y metodos semanticos.
- Repositorios como contratos del dominio.

### Arquitectura limpia y CQRS

Implementado en `src/Presupuesto.Application`:

- Separacion de capas: Domain, Application, Infrastructure, Api.
- Comandos y manejadores para escrituras.
- Consultas y manejadores para lecturas.
- Abstracciones de aplicacion: `IManejadorComando`, `IManejadorConsulta`, `IUnidadDeTrabajo`, `IServicioHashContrasena`, `IServicioTokens`.

### Infraestructura y persistencia

Implementado en `src/Presupuesto.Infrastructure`:

- EF Core con PostgreSQL.
- `PresupuestoDbContext`.
- Configuraciones de entidades.
- Repositorios concretos.
- Migracion `InitialCreate`.

### .NET Aspire

Implementado en `src/Presupuesto.AppHost`:

- Orquesta la API.
- Orquesta PostgreSQL.
- Expone PgAdmin.
- Inyecta la connection string `presupuesto`.

### Testing

Implementado en `tests/Presupuesto.Tests`:

- Pruebas unitarias de dominio.
- Pruebas de handlers CQRS.
- Prueba E2E con `Aspire.Hosting.Testing`.

### Consumo de API

Archivo principal:

```text
src/Presupuesto.Api/Presupuesto.Api.http
```

## Comandos utiles

Compilar:

```bash
dotnet build Presupuesto.slnx
```

Ejecutar tests:

```bash
dotnet test Presupuesto.slnx
```

Ejecutar con Aspire:

```bash
aspire start --project src/Presupuesto.AppHost/Presupuesto.AppHost.csproj
```

Crear nuevas migraciones:

```bash
dotnet tool run dotnet-ef migrations add NombreMigracion --project src/Presupuesto.Infrastructure/Presupuesto.Infrastructure.csproj --startup-project src/Presupuesto.Api/Presupuesto.Api.csproj --context PresupuestoDbContext --output-dir Database/Migrations
```

## Plan por fases pendiente

Estas fases parten del estado actual del proyecto. No se repiten fases ya completadas, salvo cuando sea necesario reforzar evidencia para la rubrica.

### Fase 2: Reforzar evidencia DDD del dominio

Objetivo: demostrar claramente entidades, value objects, eventos de dominio y encapsulamiento.

- Revisar que `Usuario` y `Transaccion` no expongan setters publicos innecesarios.
- Confirmar que las colecciones de eventos de dominio se expongan como solo lectura.
- Agregar o mejorar pruebas unitarias para reglas de negocio criticas.
- Revisar que los Value Objects validen correctamente datos invalidos.
- Documentar en el README los archivos exactos donde se evidencia cada patron DDD.

### Fase 3: Reforzar CQRS y casos de uso obligatorios

Objetivo: que cada flujo evaluable tenga command/query y handler propio.

- Confirmar minimo 3 casos de uso principales para exponer en la defensa:
  - Registrar usuario.
  - Crear transaccion.
  - Obtener resumen de presupuesto.
- Mantener separados comandos de escritura y consultas de lectura.
- Revisar que los handlers dependan de abstracciones y no de infraestructura concreta.
- Agregar pruebas de aplicacion para cualquier caso de uso que quede sin cobertura.

### Fase 4: Validar infraestructura, migraciones y Aspire

Objetivo: asegurar que la persistencia y la orquestacion cumplan la rubrica.

- Ejecutar `aspire doctor --non-interactive` antes de la entrega.
- Ejecutar el proyecto con `aspire start --project src/Presupuesto.AppHost/Presupuesto.AppHost.csproj`.
- Verificar desde el dashboard de Aspire que levanten API, PostgreSQL y PgAdmin.
- Confirmar que la API aplique la migracion `InitialCreate` al iniciar.
- Revisar que no haya connection strings sensibles quemadas para produccion.

### Fase 5: Completar pruebas funcionales y E2E con Aspire

Objetivo: evidenciar testing real con `Aspire.Hosting.Testing`.

- Ejecutar `dotnet test Presupuesto.slnx` con Docker Desktop iniciado.
- Confirmar que la prueba E2E levante el AppHost programaticamente.
- Validar en el test E2E el flujo completo: registro, login, crear transaccion y resumen.
- Agregar casos negativos si queda tiempo: login invalido, acceso sin token, transaccion de otro usuario.
- Guardar en la documentacion el comando exacto para ejecutar la suite.

### Fase 6: Preparar consumo API y entrega final

Objetivo: dejar el repositorio listo para revision.

- Revisar `src/Presupuesto.Api/Presupuesto.Api.http` y confirmar que cubre todos los endpoints.
- Probar manualmente las peticiones principales usando la URL mostrada por Aspire o `http://localhost:5140`.
- Actualizar README con pasos de ejecucion, pruebas y descripcion de arquitectura.
- Confirmar que no existan archivos Python, bases SQLite antiguas ni documentacion del backend anterior dentro de la entrega.
- Ejecutar revision final:

```bash
dotnet build Presupuesto.slnx
dotnet test Presupuesto.slnx
rg --files -g "*.py" -g "requirements*.txt" -g "*.db"
```
