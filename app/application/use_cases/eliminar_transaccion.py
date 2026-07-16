from sqlalchemy.orm import Session

from app.application.use_cases.obtener_transaccion import obtener_transaccion


def eliminar_transaccion(db: Session, transaccion_id: int, usuario_id: int) -> None:
    transaccion = obtener_transaccion(db, transaccion_id, usuario_id)
    db.delete(transaccion)
    db.commit()
