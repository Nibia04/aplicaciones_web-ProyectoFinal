from datetime import date

from pydantic import BaseModel, ConfigDict, EmailStr, Field

from app.domain.models import TipoTransaccion


class UsuarioCreate(BaseModel):
    nombre: str = Field(min_length=2, max_length=100)
    email: EmailStr
    password: str = Field(min_length=6)


class UsuarioOut(BaseModel):
    model_config = ConfigDict(from_attributes=True)

    id: int
    nombre: str
    email: EmailStr


class LoginRequest(BaseModel):
    email: EmailStr
    password: str


class TokenOut(BaseModel):
    access_token: str
    token_type: str = "bearer"


class TransaccionBase(BaseModel):
    monto: float = Field(gt=0)
    descripcion: str = Field(min_length=1, max_length=255)
    categoria: str = Field(min_length=1, max_length=100)
    fecha: date
    tipo: TipoTransaccion


class TransaccionCreate(TransaccionBase):
    pass


class TransaccionUpdate(BaseModel):
    monto: float | None = Field(default=None, gt=0)
    descripcion: str | None = Field(default=None, min_length=1, max_length=255)
    categoria: str | None = Field(default=None, min_length=1, max_length=100)
    fecha: date | None = None
    tipo: TipoTransaccion | None = None


class TransaccionOut(TransaccionBase):
    model_config = ConfigDict(from_attributes=True)

    id: int


class ResumenDiarioOut(BaseModel):
    fecha: date
    ingresos: float
    gastos: float
    saldo: float
    cantidad_transacciones: int


class PresupuestoOut(BaseModel):
    saldo_actual: float
    total_ingresos: float
    total_gastos: float
    resumen_diario: list[ResumenDiarioOut]
