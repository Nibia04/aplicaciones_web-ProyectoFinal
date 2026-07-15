"""Modelos de dominio puros.

Este modulo se mantiene por compatibilidad con imports existentes. Los modelos
ORM de SQLAlchemy viven en `app.infrastructure.orm_models`.
"""

from app.domain.entities import TipoTransaccion, Transaccion, Usuario

__all__ = ["TipoTransaccion", "Transaccion", "Usuario"]
