from sqlalchemy import select
from sqlalchemy.orm import Session

from app.infrastructure.orm_models import Usuario


class UsuarioRepositorySqlAlchemy:
    def __init__(self, db: Session) -> None:
        self.db = db

    def obtener_por_email(self, email: str) -> Usuario | None:
        return self.db.scalar(select(Usuario).where(Usuario.email == email))

    def obtener_por_id(self, usuario_id: int) -> Usuario | None:
        return self.db.get(Usuario, usuario_id)

    def crear(self, nombre: str, email: str, password_hash: str) -> Usuario:
        usuario = Usuario(nombre=nombre, email=email, password_hash=password_hash)
        self.db.add(usuario)
        self.db.commit()
        self.db.refresh(usuario)
        return usuario
