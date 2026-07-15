from typing import Annotated

from fastapi import APIRouter, Depends
from sqlalchemy.orm import Session

from app.api.dependencies import get_current_user
from app.application.services.presupuesto_service import construir_resumen_presupuesto
from app.application.schemas import PresupuestoOut
from app.domain.models import Usuario
from app.infrastructure.database import get_db

router = APIRouter(prefix="/presupuesto", tags=["Presupuesto"])


@router.get("/resumen", response_model=PresupuestoOut)
def obtener_resumen(
    db: Annotated[Session, Depends(get_db)],
    usuario: Annotated[Usuario, Depends(get_current_user)],
):
    return construir_resumen_presupuesto(db=db, usuario_id=usuario.id)
