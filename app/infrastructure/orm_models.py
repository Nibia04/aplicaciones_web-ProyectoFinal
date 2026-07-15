from datetime import date

from sqlalchemy import Date, Enum as SQLEnum, Float, ForeignKey, String
from sqlalchemy.orm import Mapped, mapped_column, relationship

from app.domain.entities import TipoTransaccion
from app.infrastructure.database import Base


class UsuarioModel(Base):
    __tablename__ = "usuarios"

    id: Mapped[int] = mapped_column(primary_key=True, index=True)
    nombre: Mapped[str] = mapped_column(String(100))
    email: Mapped[str] = mapped_column(String(255), unique=True, index=True)
    password_hash: Mapped[str] = mapped_column(String(255))

    transacciones: Mapped[list["TransaccionModel"]] = relationship(
        back_populates="usuario",
        cascade="all, delete-orphan",
    )


class TransaccionModel(Base):
    __tablename__ = "transacciones"

    id: Mapped[int] = mapped_column(primary_key=True, index=True)
    monto: Mapped[float] = mapped_column(Float)
    descripcion: Mapped[str] = mapped_column(String(255))
    categoria: Mapped[str] = mapped_column(String(100), index=True)
    fecha: Mapped[date] = mapped_column(Date, default=date.today, index=True)
    tipo: Mapped[TipoTransaccion] = mapped_column(SQLEnum(TipoTransaccion), index=True)
    usuario_id: Mapped[int] = mapped_column(ForeignKey("usuarios.id"), index=True)

    usuario: Mapped[UsuarioModel] = relationship(back_populates="transacciones")


# Alias temporales para mantener las rutas actuales simples hasta la Fase 3.
Usuario = UsuarioModel
Transaccion = TransaccionModel
