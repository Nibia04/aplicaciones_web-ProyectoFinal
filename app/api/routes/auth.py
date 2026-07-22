from typing import Annotated

from fastapi import APIRouter, Depends, status

from app.api.dependencies import get_password_hasher, get_token_service, get_usuario_repository
from app.application.ports import PasswordHasher, TokenService, UsuarioRepository
from app.application.schemas import LoginRequest, TokenOut, UsuarioCreate, UsuarioOut
from app.application.use_cases.login_usuario import login_usuario
from app.application.use_cases.registrar_usuario import registrar_usuario as registrar_usuario_use_case

router = APIRouter(prefix="/auth", tags=["Autenticacion"])


@router.post("/registro", response_model=UsuarioOut, status_code=status.HTTP_201_CREATED)
def registrar_usuario(
    payload: UsuarioCreate,
    usuarios: Annotated[UsuarioRepository, Depends(get_usuario_repository)],
    password_hasher: Annotated[PasswordHasher, Depends(get_password_hasher)],
):
    return registrar_usuario_use_case(
        payload=payload,
        usuarios=usuarios,
        password_hasher=password_hasher,
    )


@router.post("/login", response_model=TokenOut)
def login(
    payload: LoginRequest,
    usuarios: Annotated[UsuarioRepository, Depends(get_usuario_repository)],
    password_hasher: Annotated[PasswordHasher, Depends(get_password_hasher)],
    token_service: Annotated[TokenService, Depends(get_token_service)],
):
    return login_usuario(
        payload=payload,
        usuarios=usuarios,
        password_hasher=password_hasher,
        token_service=token_service,
    )
