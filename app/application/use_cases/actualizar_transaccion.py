from app.application.ports import TransaccionRepository
from app.application.schemas import TransaccionUpdate
from app.application.use_cases.obtener_transaccion import obtener_transaccion


def actualizar_transaccion(
    transaccion_id: int,
    payload: TransaccionUpdate,
    transacciones: TransaccionRepository,
    usuario_id: int,
):
    transaccion = obtener_transaccion(transacciones, transaccion_id, usuario_id)
    cambios = payload.model_dump(exclude_unset=True)
    return transacciones.actualizar(transaccion, cambios)
