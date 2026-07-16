from sqlalchemy.orm import Session

from app.application.schemas import TransaccionUpdate
from app.application.use_cases.obtener_transaccion import obtener_transaccion
from app.infrastructure.orm_models import Transaccion


def actualizar_transaccion(
    transaccion_id: int,
    payload: TransaccionUpdate,
    db: Session,
    usuario_id: int,
) -> Transaccion:
    transaccion = obtener_transaccion(db, transaccion_id, usuario_id)
    for campo, valor in payload.model_dump(exclude_unset=True).items():
        setattr(transaccion, campo, valor)

    db.commit()
    db.refresh(transaccion)
    return transaccion
