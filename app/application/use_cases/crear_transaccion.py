from app.application.ports import TransaccionRepository
from app.application.schemas import TransaccionCreate


def crear_transaccion(
    payload: TransaccionCreate,
    transacciones: TransaccionRepository,
    usuario_id: int,
):
    return transacciones.crear(**payload.model_dump(), usuario_id=usuario_id)
