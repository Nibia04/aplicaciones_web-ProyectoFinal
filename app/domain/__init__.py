
"""Capa Domain.

Responsabilidad:
- Contener entidades, value objects, reglas de negocio y contratos centrales.

Regla:
- Esta capa debe ser independiente de FastAPI, SQLAlchemy, JWT y cualquier
  detalle tecnico.

Nota:
- Los modelos ORM de SQLAlchemy viven en `app.infrastructure.orm_models`.
"""
