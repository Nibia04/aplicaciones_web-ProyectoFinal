from typing import Annotated

from fastapi import Depends, HTTPException, status
from fastapi.security import OAuth2PasswordBearer
from sqlalchemy.orm import Session

from app.application.ports import PasswordHasher, TokenService, TransaccionRepository, UsuarioRepository
from app.infrastructure.database import get_db
from app.infrastructure.orm_models import Usuario
from app.infrastructure.repositories.transaccion_repository_sqlalchemy import (
    TransaccionRepositorySqlAlchemy,
)
from app.infrastructure.repositories.usuario_repository_sqlalchemy import UsuarioRepositorySqlAlchemy
from app.infrastructure.security import JwtTokenService, PasslibPasswordHasher

oauth2_scheme = OAuth2PasswordBearer(tokenUrl="/auth/login")


def get_usuario_repository(
    db: Annotated[Session, Depends(get_db)],
) -> UsuarioRepository:
    return UsuarioRepositorySqlAlchemy(db)


def get_transaccion_repository(
    db: Annotated[Session, Depends(get_db)],
) -> TransaccionRepository:
    return TransaccionRepositorySqlAlchemy(db)


def get_password_hasher() -> PasswordHasher:
    return PasslibPasswordHasher()


def get_token_service() -> TokenService:
    return JwtTokenService()


def get_current_user(
    token: Annotated[str, Depends(oauth2_scheme)],
    usuarios: Annotated[UsuarioRepository, Depends(get_usuario_repository)],
    token_service: Annotated[TokenService, Depends(get_token_service)],
) -> Usuario:
    user_id = token_service.decodificar_access_token(token)
    if user_id is None:
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Token invalido o expirado",
            headers={"WWW-Authenticate": "Bearer"},
        )

    user = usuarios.obtener_por_id(int(user_id))
    if user is None:
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Usuario no encontrado",
            headers={"WWW-Authenticate": "Bearer"},
        )
    return user
