from datetime import date
from enum import Enum

from sqlalchemy import Date, Enum as SQLEnum, Float, ForeignKey, String
from sqlalchemy.orm import Mapped, mapped_column, relationship

from app.infrastructure.database import Base


class TipoTransaccion(str, Enum):
    ingreso = "ingreso"
    gasto = "gasto"


class Usuario(Base):
    __tablename__ = "usuarios"

    id: Mapped[int] = mapped_column(primary_key=True, index=True)
    nombre: Mapped[str] = mapped_column(String(100))
    email: Mapped[str] = mapped_column(String(255), unique=True, index=True)
    password_hash: Mapped[str] = mapped_column(String(255))

    transacciones: Mapped[list["Transaccion"]] = relationship(
        back_populates="usuario",
        cascade="all, delete-orphan",
    )


class Transaccion(Base):
    __tablename__ = "transacciones"

    id: Mapped[int] = mapped_column(primary_key=True, index=True)
    monto: Mapped[float] = mapped_column(Float)
    descripcion: Mapped[str] = mapped_column(String(255))
    categoria: Mapped[str] = mapped_column(String(100), index=True)
    fecha: Mapped[date] = mapped_column(Date, default=date.today, index=True)
    tipo: Mapped[TipoTransaccion] = mapped_column(SQLEnum(TipoTransaccion), index=True)
    usuario_id: Mapped[int] = mapped_column(ForeignKey("usuarios.id"), index=True)

    usuario: Mapped[Usuario] = relationship(back_populates="transacciones")
