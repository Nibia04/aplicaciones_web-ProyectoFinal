
"""Capa Domain.

Responsabilidad:
- Contener entidades, value objects, reglas de negocio y contratos centrales.

Regla:
- Esta capa debe ser independiente de FastAPI, SQLAlchemy, JWT y cualquier
  detalle tecnico.

Nota:
- Actualmente `models.py` aun contiene modelos SQLAlchemy. Esa deuda se
  corrige en la Fase 2 moviendo el ORM a infraestructura.
"""
