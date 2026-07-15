
"""Capa API.

Responsabilidad:
- Definir rutas HTTP, dependencias de FastAPI y traduccion de errores a
  respuestas HTTP.

Regla:
- Esta capa puede llamar a casos de uso de `app.application`.
- No debe contener reglas de negocio ni consultas complejas de persistencia.
"""
