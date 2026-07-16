from sqlalchemy.orm import Session

from app.application.schemas import TransaccionCreate
from app.infrastructure.orm_models import Transaccion


def crear_transaccion(
    payload: TransaccionCreate,
    db: Session,
    usuario_id: int,
) -> Transaccion:
    transaccion = Transaccion(**payload.model_dump(), usuario_id=usuario_id)
    db.add(transaccion)
    db.commit()
    db.refresh(transaccion)
    return transaccion
