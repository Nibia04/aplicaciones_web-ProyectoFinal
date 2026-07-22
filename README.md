# Gestor de Presupuesto Diario

Backend de tema libre migrado a **.NET 10**, **ASP.NET Core**, **Entity Framework Core**, **PostgreSQL**, **JWT** y **.NET Aspire**. El sistema permite registrar usuarios, iniciar sesion, gestionar ingresos/gastos y consultar un resumen de presupuesto.

La implementacion principal para la rubrica esta en la solucion .NET:

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

## Arquitectura

```text
Api -> Application -> Domain
       |
       v
 Infrastructure

AppHost -> Api + PostgreSQL
```

### Domain

`src/Presupuesto.Domain` contiene el nucleo DDD sin dependencias externas:

- Entidades: `Usuario`, `Transaccion`.
- Value Objects: `Email`, `Nombre`, `Dinero`, `Descripcion`, `Categoria`.
- Eventos de dominio: `UsuarioRegistradoDomainEvent`, `TransaccionCreadaDomainEvent`.
- Encapsulamiento: setters privados, metodos semanticos y colecciones de solo lectura.
- Contratos de repositorio: `IUsuarioRepository`, `ITransaccionRepository`.

### Application

`src/Presupuesto.Application` contiene CQRS:

- Commands:
  - `RegistrarUsuarioCommand`
  - `LoginUsuarioCommand`
  - `CrearTransaccionCommand`
  - `ActualizarTransaccionCommand`
  - `EliminarTransaccionCommand`
- Queries:
  - `ListarTransaccionesQuery`
  - `ObtenerTransaccionQuery`
  - `ObtenerResumenPresupuestoQuery`
- Handlers por caso de uso.
- Abstracciones: `ICommandHandler`, `IQueryHandler`, `IUnitOfWork`, `IPasswordHasher`, `ITokenService`.

### Infrastructure

`src/Presupuesto.Infrastructure` contiene detalles tecnicos:

- `PresupuestoDbContext`.
- Configuraciones EF Core.
- Repositorios concretos.
- Migraciones EF Core.
- Hash de passwords.
- JWT.

### Api

`src/Presupuesto.Api` expone endpoints HTTP:

```text
GET    /
GET    /health
POST   /auth/registro
POST   /auth/login
POST   /transacciones
GET    /transacciones
GET    /transacciones/{id}
PUT    /transacciones/{id}
DELETE /transacciones/{id}
GET    /presupuesto/resumen
```

### Aspire

`src/Presupuesto.AppHost` orquesta:

- API ASP.NET Core.
- PostgreSQL.
- PgAdmin.
- Variables de entorno y connection string `presupuesto`.

## Ejecutar

Requisito: Docker Desktop debe estar corriendo para levantar PostgreSQL con Aspire.

```bash
aspire start --project src/Presupuesto.AppHost/Presupuesto.AppHost.csproj
```

La API aplica migraciones EF Core automaticamente al iniciar.

## Tests

```bash
dotnet test Presupuesto.slnx
```

Incluye:

- Pruebas unitarias de dominio.
- Pruebas de handlers CQRS.
- Prueba E2E con `Aspire.Hosting.Testing`.

Nota: si Docker no esta corriendo, la prueba E2E Aspire no levanta contenedores y retorna sin ejecutar el flujo externo. Con Docker activo, levanta el entorno Aspire programaticamente.

## Consumo HTTP

Archivo incluido:

```text
src/Presupuesto.Api/Presupuesto.Api.http
```
