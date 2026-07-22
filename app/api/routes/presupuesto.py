from typing import Annotated

from fastapi import APIRouter, Depends

from app.api.dependencies import get_current_user, get_transaccion_repository
from app.application.ports import TransaccionRepository
from app.application.schemas import PresupuestoOut
from app.application.use_cases.obtener_resumen_presupuesto import obtener_resumen_presupuesto
from app.infrastructure.orm_models import Usuario

router = APIRouter(prefix="/presupuesto", tags=["Presupuesto"])


@router.get("/resumen", response_model=PresupuestoOut)
def obtener_resumen(
    transacciones: Annotated[TransaccionRepository, Depends(get_transaccion_repository)],
    usuario: Annotated[Usuario, Depends(get_current_user)],
):
    return obtener_resumen_presupuesto(transacciones=transacciones, usuario_id=usuario.id)
