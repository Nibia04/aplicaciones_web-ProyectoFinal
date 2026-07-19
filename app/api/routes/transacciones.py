from typing import Annotated

from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session

from app.api.dependencies import get_current_user
from app.application.errors import TransaccionNoEncontradaError
from app.application.schemas import TransaccionCreate, TransaccionOut, TransaccionUpdate
from app.application.use_cases.actualizar_transaccion import actualizar_transaccion as actualizar_transaccion_use_case
from app.application.use_cases.crear_transaccion import crear_transaccion as crear_transaccion_use_case
from app.application.use_cases.eliminar_transaccion import eliminar_transaccion as eliminar_transaccion_use_case
from app.application.use_cases.listar_transacciones import listar_transacciones as listar_transacciones_use_case
from app.application.use_cases.obtener_transaccion import obtener_transaccion as obtener_transaccion_use_case
from app.infrastructure.database import get_db
from app.infrastructure.orm_models import Usuario
from app.infrastructure.repositories.transaccion_repository_sqlalchemy import (
    TransaccionRepositorySqlAlchemy,
)

router = APIRouter(prefix="/transacciones", tags=["Transacciones"])


@router.post("", response_model=TransaccionOut, status_code=status.HTTP_201_CREATED)
def crear_transaccion(
    payload: TransaccionCreate,
    db: Annotated[Session, Depends(get_db)],
    usuario: Annotated[Usuario, Depends(get_current_user)],
):
    transacciones = TransaccionRepositorySqlAlchemy(db)
    return crear_transaccion_use_case(
        payload=payload,
        transacciones=transacciones,
        usuario_id=usuario.id,
    )


@router.get("", response_model=list[TransaccionOut])
def listar_transacciones(
    db: Annotated[Session, Depends(get_db)],
    usuario: Annotated[Usuario, Depends(get_current_user)],
):
    transacciones = TransaccionRepositorySqlAlchemy(db)
    return listar_transacciones_use_case(transacciones=transacciones, usuario_id=usuario.id)


@router.get("/{transaccion_id}", response_model=TransaccionOut)
def obtener_transaccion(
    transaccion_id: int,
    db: Annotated[Session, Depends(get_db)],
    usuario: Annotated[Usuario, Depends(get_current_user)],
):
    transacciones = TransaccionRepositorySqlAlchemy(db)
    try:
        return obtener_transaccion_use_case(
            transacciones=transacciones,
            transaccion_id=transaccion_id,
            usuario_id=usuario.id,
        )
    except TransaccionNoEncontradaError:
        raise HTTPException(status_code=404, detail="Transaccion no encontrada")


@router.put("/{transaccion_id}", response_model=TransaccionOut)
def actualizar_transaccion(
    transaccion_id: int,
    payload: TransaccionUpdate,
    db: Annotated[Session, Depends(get_db)],
    usuario: Annotated[Usuario, Depends(get_current_user)],
):
    transacciones = TransaccionRepositorySqlAlchemy(db)
    try:
        return actualizar_transaccion_use_case(
            transaccion_id=transaccion_id,
            payload=payload,
            transacciones=transacciones,
            usuario_id=usuario.id,
        )
    except TransaccionNoEncontradaError:
        raise HTTPException(status_code=404, detail="Transaccion no encontrada")


@router.delete("/{transaccion_id}", status_code=status.HTTP_204_NO_CONTENT)
def eliminar_transaccion(
    transaccion_id: int,
    db: Annotated[Session, Depends(get_db)],
    usuario: Annotated[Usuario, Depends(get_current_user)],
):
    transacciones = TransaccionRepositorySqlAlchemy(db)
    try:
        eliminar_transaccion_use_case(
            transacciones=transacciones,
            transaccion_id=transaccion_id,
            usuario_id=usuario.id,
        )
    except TransaccionNoEncontradaError:
        raise HTTPException(status_code=404, detail="Transaccion no encontrada")
    return None
