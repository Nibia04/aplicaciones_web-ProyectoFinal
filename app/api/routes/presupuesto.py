from typing import Annotated

from fastapi import APIRouter, Depends
from sqlalchemy.orm import Session

from app.api.dependencies import get_current_user
from app.application.schemas import PresupuestoOut
from app.application.use_cases.obtener_resumen_presupuesto import obtener_resumen_presupuesto
from app.infrastructure.database import get_db
from app.infrastructure.orm_models import Usuario
from app.infrastructure.repositories.transaccion_repository_sqlalchemy import (
    TransaccionRepositorySqlAlchemy,
)

router = APIRouter(prefix="/presupuesto", tags=["Presupuesto"])


@router.get("/resumen", response_model=PresupuestoOut)
def obtener_resumen(
    db: Annotated[Session, Depends(get_db)],
    usuario: Annotated[Usuario, Depends(get_current_user)],
):
    transacciones = TransaccionRepositorySqlAlchemy(db)
    return obtener_resumen_presupuesto(transacciones=transacciones, usuario_id=usuario.id)
