from typing import Annotated

from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session

from app.application.errors import CredencialesInvalidasError, EmailYaRegistradoError
from app.application.schemas import LoginRequest, TokenOut, UsuarioCreate, UsuarioOut
from app.application.use_cases.login_usuario import login_usuario
from app.application.use_cases.registrar_usuario import registrar_usuario as registrar_usuario_use_case
from app.infrastructure.database import get_db

router = APIRouter(prefix="/auth", tags=["Autenticacion"])


@router.post("/registro", response_model=UsuarioOut, status_code=status.HTTP_201_CREATED)
def registrar_usuario(payload: UsuarioCreate, db: Annotated[Session, Depends(get_db)]):
    try:
        return registrar_usuario_use_case(payload=payload, db=db)
    except EmailYaRegistradoError:
        raise HTTPException(status_code=409, detail="El email ya esta registrado")


@router.post("/login", response_model=TokenOut)
def login(payload: LoginRequest, db: Annotated[Session, Depends(get_db)]):
    try:
        return login_usuario(payload=payload, db=db)
    except CredencialesInvalidasError:
        raise HTTPException(status_code=401, detail="Credenciales invalidas")
