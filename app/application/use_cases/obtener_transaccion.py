from typing import Any

from app.application.errors import TransaccionNoEncontradaError
from app.application.ports import TransaccionRepository


def obtener_transaccion(
    transacciones: TransaccionRepository,
    transaccion_id: int,
    usuario_id: int,
) -> Any:
    transaccion = transacciones.obtener_por_id_y_usuario(transaccion_id, usuario_id)
    if transaccion is None:
        raise TransaccionNoEncontradaError("Transaccion no encontrada")
    return transaccion
