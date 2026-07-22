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

## Estado de verificacion

- Los eventos de dominio se despachan despues de una persistencia exitosa y son atendidos por manejadores desacoplados.
- Una prueba de arquitectura verifica que Domain no referencia Application, Infrastructure, API ni EF Core.
- La prueba E2E levanta el AppHost con `Aspire.Hosting.Testing`, espera la salud de la API y ejecuta autorizacion, registro, login, CRUD y resumen contra endpoints reales.
- El archivo `.http` guarda automaticamente el JWT y el identificador creado para ejecutar el flujo en orden.
- Ultima verificacion local: 10 pruebas superadas, 0 fallidas y 0 omitidas.

Docker Desktop debe estar iniciado antes de ejecutar `dotnet test Presupuesto.slnx`; una infraestructura no disponible produce un fallo real y nunca se reporta como una prueba aprobada.
