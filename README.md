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
