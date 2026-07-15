from typing import Annotated

from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy import select
from sqlalchemy.orm import Session

from app.application.schemas import LoginRequest, TokenOut, UsuarioCreate, UsuarioOut
from app.infrastructure.database import get_db
from app.infrastructure.orm_models import Usuario
from app.infrastructure.security import create_access_token, hash_password, verify_password

router = APIRouter(prefix="/auth", tags=["Autenticacion"])


@router.post("/registro", response_model=UsuarioOut, status_code=status.HTTP_201_CREATED)
def registrar_usuario(payload: UsuarioCreate, db: Annotated[Session, Depends(get_db)]):
    existe = db.scalar(select(Usuario).where(Usuario.email == payload.email))
    if existe:
        raise HTTPException(status_code=409, detail="El email ya esta registrado")

    usuario = Usuario(
        nombre=payload.nombre,
        email=payload.email,
        password_hash=hash_password(payload.password),
    )
    db.add(usuario)
    db.commit()
    db.refresh(usuario)
    return usuario


@router.post("/login", response_model=TokenOut)
def login(payload: LoginRequest, db: Annotated[Session, Depends(get_db)]):
    usuario = db.scalar(select(Usuario).where(Usuario.email == payload.email))
    if not usuario or not verify_password(payload.password, usuario.password_hash):
        raise HTTPException(status_code=401, detail="Credenciales invalidas")

    token = create_access_token(subject=str(usuario.id))
    return TokenOut(access_token=token)
