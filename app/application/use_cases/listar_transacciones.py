from typing import Any

from app.application.ports import TransaccionRepository


def listar_transacciones(
    transacciones: TransaccionRepository,
    usuario_id: int,
) -> list[Any]:
    return transacciones.listar_por_usuario(usuario_id)
