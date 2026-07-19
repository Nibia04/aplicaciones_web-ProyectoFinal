# Gestor de Presupuesto Diario

Backend con FastAPI, SQLite, SQLAlchemy y JWT para registrar transacciones de ingresos y gastos por usuario.

## Estructura

```text
app/
  main.py
  api/
    dependencies.py
    routes/
      auth.py
      transacciones.py
      presupuesto.py
  application/
    errors.py
    ports.py
    schemas.py
    services/
      presupuesto_service.py
    use_cases/
      registrar_usuario.py
      login_usuario.py
      crear_transaccion.py
      listar_transacciones.py
      obtener_transaccion.py
      actualizar_transaccion.py
      eliminar_transaccion.py
      obtener_resumen_presupuesto.py
  domain/
    entities.py
    models.py
  infrastructure/
    database.py
    orm_models.py
    repositories/
      usuario_repository_sqlalchemy.py
      transaccion_repository_sqlalchemy.py
    security.py
tests/
  test_api.py
requirements.txt
```

Esta organizacion es parecida a una arquitectura por capas:

- `api`: endpoints de FastAPI, rutas y dependencias HTTP.
- `application`: DTOs/esquemas y servicios con casos de uso.
- `domain`: entidades y reglas centrales del negocio, como `Usuario` y `Transaccion`.
- `infrastructure`: conexion a base de datos, seguridad, JWT y herramientas externas.

## Responsabilidades por capa

El proyecto se esta ordenando para acercarse a una arquitectura limpia por capas:

```text
API -> Application -> Domain
        |
        v
 Infrastructure
```

### `app/api`

Contiene rutas HTTP, dependencias de FastAPI y traduccion de errores a respuestas HTTP.
Esta capa debe ser delgada: recibe solicitudes, llama a casos de uso y devuelve respuestas.

### `app/application`

Contiene casos de uso, DTOs, puertos e interfaces de aplicacion. Aqui vive la orquestacion de acciones como registrar usuario, crear transaccion, listar transacciones u obtener resumen de presupuesto.

### `app/domain`

Contiene entidades y reglas centrales del negocio. En una arquitectura limpia estricta no debe depender de FastAPI, SQLAlchemy, JWT ni detalles de infraestructura.

Estado actual: el dominio ya no contiene modelos SQLAlchemy. Las entidades puras estan en `app/domain/entities.py` y `app/domain/models.py` queda como modulo de compatibilidad.

### `app/infrastructure`

Contiene detalles tecnicos: base de datos, modelos ORM, seguridad, JWT, configuracion y repositorios concretos.

### `tests`

Contiene pruebas automatizadas. Actualmente hay pruebas de API; en siguientes fases se agregaran pruebas por capa.

## Estado de mejora arquitectonica

- Fase 1 completada: estructura base y responsabilidades de capas documentadas.
- Fase 2 completada: dominio separado de SQLAlchemy; los modelos ORM viven en infraestructura.
- Fase 3 completada: la logica principal de rutas fue movida a casos de uso en `app/application/use_cases`.
- Fase 4 completada: casos de uso desacoplados de SQLAlchemy mediante puertos e implementaciones de repositorio.
- Siguiente paso: mejorar la API y estandarizar respuestas/errores.

## Ejecutar

```bash
.\.venv\Scripts\pip.exe install -r requirements.txt
.\.venv\Scripts\python.exe -m uvicorn app.main:app --reload
```

La base SQLite se crea automaticamente en `presupuesto.db`.

## Tests

```bash
.\.venv\Scripts\pip.exe install -r requirements-dev.txt
.\.venv\Scripts\python.exe -m pytest
```

Los tests usan una base SQLite temporal llamada `test_presupuesto.db`.

## Flujo rapido en Postman

1. Registrar usuario:

```http
POST http://127.0.0.1:8000/auth/registro
```

```json
{
  "nombre": "Ana",
  "email": "ana@example.com",
  "password": "secreto123"
}
```

2. Iniciar sesion:

```http
POST http://127.0.0.1:8000/auth/login
```

```json
{
  "email": "ana@example.com",
  "password": "secreto123"
}
```

3. Copiar el `access_token` y usarlo en Authorization como `Bearer Token`.

4. Crear transaccion:

```http
POST http://127.0.0.1:8000/transacciones
```

```json
{
  "monto": 25.5,
  "descripcion": "Almuerzo",
  "categoria": "Comida",
  "fecha": "2026-07-14",
  "tipo": "gasto"
}
```

5. Consultar resumen:

```http
GET http://127.0.0.1:8000/presupuesto/resumen
```
