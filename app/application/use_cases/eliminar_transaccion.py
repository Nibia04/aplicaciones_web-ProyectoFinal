from app.application.ports import TransaccionRepository
from app.application.use_cases.obtener_transaccion import obtener_transaccion


def eliminar_transaccion(
    transacciones: TransaccionRepository,
    transaccion_id: int,
    usuario_id: int,
) -> None:
    transaccion = obtener_transaccion(transacciones, transaccion_id, usuario_id)
    transacciones.eliminar(transaccion)
