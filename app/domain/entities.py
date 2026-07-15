from dataclasses import dataclass
from datetime import date
from enum import Enum


class TipoTransaccion(str, Enum):
    ingreso = "ingreso"
    gasto = "gasto"


@dataclass(slots=True)
class Usuario:
    id: int | None
    nombre: str
    email: str
    password_hash: str


@dataclass(slots=True)
class Transaccion:
    id: int | None
    monto: float
    descripcion: str
    categoria: str
    fecha: date
    tipo: TipoTransaccion
    usuario_id: int
