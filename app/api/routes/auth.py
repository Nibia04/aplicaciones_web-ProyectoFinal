from typing import Annotated

from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session

from app.application.errors import CredencialesInvalidasError, EmailYaRegistradoError
from app.application.schemas import LoginRequest, TokenOut, UsuarioCreate, UsuarioOut
from app.application.use_cases.login_usuario import login_usuario
from app.application.use_cases.registrar_usuario import registrar_usuario as registrar_usuario_use_case
from app.infrastructure.database import get_db
from app.infrastructure.repositories.usuario_repository_sqlalchemy import UsuarioRepositorySqlAlchemy
from app.infrastructure.security import JwtTokenService, PasslibPasswordHasher

router = APIRouter(prefix="/auth", tags=["Autenticacion"])


@router.post("/registro", response_model=UsuarioOut, status_code=status.HTTP_201_CREATED)
def registrar_usuario(payload: UsuarioCreate, db: Annotated[Session, Depends(get_db)]):
    usuarios = UsuarioRepositorySqlAlchemy(db)
    password_hasher = PasslibPasswordHasher()
    try:
        return registrar_usuario_use_case(
            payload=payload,
            usuarios=usuarios,
            password_hasher=password_hasher,
        )
    except EmailYaRegistradoError:
        raise HTTPException(status_code=409, detail="El email ya esta registrado")


@router.post("/login", response_model=TokenOut)
def login(payload: LoginRequest, db: Annotated[Session, Depends(get_db)]):
    usuarios = UsuarioRepositorySqlAlchemy(db)
    password_hasher = PasslibPasswordHasher()
    token_service = JwtTokenService()
    try:
        return login_usuario(
            payload=payload,
            usuarios=usuarios,
            password_hasher=password_hasher,
            token_service=token_service,
        )
    except CredencialesInvalidasError:
        raise HTTPException(status_code=401, detail="Credenciales invalidas")
