from sqlalchemy.orm import Session

from app.application.schemas import PresupuestoOut
from app.application.services.presupuesto_service import construir_resumen_presupuesto


def obtener_resumen_presupuesto(db: Session, usuario_id: int) -> PresupuestoOut:
    return construir_resumen_presupuesto(db=db, usuario_id=usuario_id)
