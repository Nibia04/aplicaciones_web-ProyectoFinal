from sqlalchemy import select
from sqlalchemy.orm import Session

from app.infrastructure.orm_models import Transaccion


def listar_transacciones(db: Session, usuario_id: int) -> list[Transaccion]:
    return db.scalars(
        select(Transaccion)
        .where(Transaccion.usuario_id == usuario_id)
        .order_by(Transaccion.fecha.desc(), Transaccion.id.desc())
    ).all()
