from sqlalchemy import select
from sqlalchemy.orm import Session

from app.application.errors import CredencialesInvalidasError
from app.application.schemas import LoginRequest, TokenOut
from app.infrastructure.orm_models import Usuario
from app.infrastructure.security import create_access_token, verify_password


def login_usuario(payload: LoginRequest, db: Session) -> TokenOut:
    usuario = db.scalar(select(Usuario).where(Usuario.email == payload.email))
    if not usuario or not verify_password(payload.password, usuario.password_hash):
        raise CredencialesInvalidasError("Credenciales invalidas")

    token = create_access_token(subject=str(usuario.id))
    return TokenOut(access_token=token)
