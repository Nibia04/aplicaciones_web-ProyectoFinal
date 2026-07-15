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
    schemas.py
    services/
      presupuesto_service.py
  domain/
    models.py
  infrastructure/
    database.py
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

Contiene casos de uso, DTOs y servicios de aplicacion. Aqui debe vivir la orquestacion de acciones como registrar usuario, crear transaccion, listar transacciones u obtener resumen de presupuesto.

### `app/domain`

Contiene entidades y reglas centrales del negocio. En una arquitectura limpia estricta no debe depender de FastAPI, SQLAlchemy, JWT ni detalles de infraestructura.

Nota actual: `app/domain/models.py` todavia usa SQLAlchemy. Esa deuda queda identificada para la siguiente fase.

### `app/infrastructure`

Contiene detalles tecnicos: base de datos, ORM, seguridad, JWT, configuracion y repositorios concretos.

### `tests`

Contiene pruebas automatizadas. Actualmente hay pruebas de API; en siguientes fases se agregaran pruebas por capa.

## Estado de mejora arquitectonica

- Fase 1 completada: estructura base y responsabilidades de capas documentadas.
- Siguiente paso: separar el dominio de SQLAlchemy para que `domain` quede independiente.

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
