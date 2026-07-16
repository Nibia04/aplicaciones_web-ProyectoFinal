from sqlalchemy import select
from sqlalchemy.orm import Session

from app.application.errors import TransaccionNoEncontradaError
from app.infrastructure.orm_models import Transaccion


def obtener_transaccion(
    db: Session,
    transaccion_id: int,
    usuario_id: int,
) -> Transaccion:
    transaccion = db.scalar(
        select(Transaccion).where(
            Transaccion.id == transaccion_id,
            Transaccion.usuario_id == usuario_id,
        )
    )
    if transaccion is None:
        raise TransaccionNoEncontradaError("Transaccion no encontrada")
    return transaccion
