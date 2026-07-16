from sqlalchemy import select
from sqlalchemy.orm import Session

from app.application.errors import EmailYaRegistradoError
from app.application.schemas import UsuarioCreate
from app.infrastructure.orm_models import Usuario
from app.infrastructure.security import hash_password


def registrar_usuario(payload: UsuarioCreate, db: Session) -> Usuario:
    existe = db.scalar(select(Usuario).where(Usuario.email == payload.email))
    if existe:
        raise EmailYaRegistradoError("El email ya esta registrado")

    usuario = Usuario(
        nombre=payload.nombre,
        email=payload.email,
        password_hash=hash_password(payload.password),
    )
    db.add(usuario)
    db.commit()
    db.refresh(usuario)
    return usuario
