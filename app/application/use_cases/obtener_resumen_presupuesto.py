from app.application.ports import TransaccionRepository
from app.application.schemas import PresupuestoOut
from app.application.services.presupuesto_service import construir_resumen_presupuesto


def obtener_resumen_presupuesto(
    transacciones: TransaccionRepository,
    usuario_id: int,
) -> PresupuestoOut:
    transacciones_usuario = transacciones.listar_por_usuario_orden_fecha(usuario_id)
    return construir_resumen_presupuesto(transacciones_usuario)
