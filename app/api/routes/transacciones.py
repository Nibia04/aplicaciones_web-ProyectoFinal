from typing import Annotated

from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy import select
from sqlalchemy.orm import Session

from app.api.dependencies import get_current_user
from app.application.schemas import TransaccionCreate, TransaccionOut, TransaccionUpdate
from app.infrastructure.database import get_db
from app.infrastructure.orm_models import Transaccion, Usuario

router = APIRouter(prefix="/transacciones", tags=["Transacciones"])


@router.post("", response_model=TransaccionOut, status_code=status.HTTP_201_CREATED)
def crear_transaccion(
    payload: TransaccionCreate,
    db: Annotated[Session, Depends(get_db)],
    usuario: Annotated[Usuario, Depends(get_current_user)],
):
    transaccion = Transaccion(**payload.model_dump(), usuario_id=usuario.id)
    db.add(transaccion)
    db.commit()
    db.refresh(transaccion)
    return transaccion


@router.get("", response_model=list[TransaccionOut])
def listar_transacciones(
    db: Annotated[Session, Depends(get_db)],
    usuario: Annotated[Usuario, Depends(get_current_user)],
):
    return db.scalars(
        select(Transaccion)
        .where(Transaccion.usuario_id == usuario.id)
        .order_by(Transaccion.fecha.desc(), Transaccion.id.desc())
    ).all()


@router.get("/{transaccion_id}", response_model=TransaccionOut)
def obtener_transaccion(
    transaccion_id: int,
    db: Annotated[Session, Depends(get_db)],
    usuario: Annotated[Usuario, Depends(get_current_user)],
):
    transaccion = _get_transaccion_usuario(db, transaccion_id, usuario.id)
    return transaccion


@router.put("/{transaccion_id}", response_model=TransaccionOut)
def actualizar_transaccion(
    transaccion_id: int,
    payload: TransaccionUpdate,
    db: Annotated[Session, Depends(get_db)],
    usuario: Annotated[Usuario, Depends(get_current_user)],
):
    transaccion = _get_transaccion_usuario(db, transaccion_id, usuario.id)
    for campo, valor in payload.model_dump(exclude_unset=True).items():
        setattr(transaccion, campo, valor)

    db.commit()
    db.refresh(transaccion)
    return transaccion


@router.delete("/{transaccion_id}", status_code=status.HTTP_204_NO_CONTENT)
def eliminar_transaccion(
    transaccion_id: int,
    db: Annotated[Session, Depends(get_db)],
    usuario: Annotated[Usuario, Depends(get_current_user)],
):
    transaccion = _get_transaccion_usuario(db, transaccion_id, usuario.id)
    db.delete(transaccion)
    db.commit()
    return None


def _get_transaccion_usuario(db: Session, transaccion_id: int, usuario_id: int) -> Transaccion:
    transaccion = db.scalar(
        select(Transaccion).where(
            Transaccion.id == transaccion_id,
            Transaccion.usuario_id == usuario_id,
        )
    )
    if transaccion is None:
        raise HTTPException(status_code=404, detail="Transaccion no encontrada")
    return transaccion
