from dataclasses import dataclass
from datetime import date
from typing import Any

from app.domain.entities import TipoTransaccion


@dataclass
class UsuarioFake:
    id: int
    nombre: str
    email: str
    password_hash: str


@dataclass
class TransaccionFake:
    id: int
    monto: float
    descripcion: str
    categoria: str
    fecha: date
    tipo: TipoTransaccion
    usuario_id: int


class UsuarioRepositoryFake:
    def __init__(self) -> None:
        self.usuarios: dict[int, UsuarioFake] = {}
        self.siguiente_id = 1

    def obtener_por_email(self, email: str) -> UsuarioFake | None:
        return next((u for u in self.usuarios.values() if u.email == email), None)

    def obtener_por_id(self, usuario_id: int) -> UsuarioFake | None:
        return self.usuarios.get(usuario_id)

    def crear(self, nombre: str, email: str, password_hash: str) -> UsuarioFake:
        usuario = UsuarioFake(
            id=self.siguiente_id,
            nombre=nombre,
            email=email,
            password_hash=password_hash,
        )
        self.usuarios[usuario.id] = usuario
        self.siguiente_id += 1
        return usuario


class TransaccionRepositoryFake:
    def __init__(self) -> None:
        self.transacciones: dict[int, TransaccionFake] = {}
        self.siguiente_id = 1

    def crear(
        self,
        monto: float,
        descripcion: str,
        categoria: str,
        fecha: date,
        tipo: TipoTransaccion,
        usuario_id: int,
    ) -> TransaccionFake:
        transaccion = TransaccionFake(
            id=self.siguiente_id,
            monto=monto,
            descripcion=descripcion,
            categoria=categoria,
            fecha=fecha,
            tipo=tipo,
            usuario_id=usuario_id,
        )
        self.transacciones[transaccion.id] = transaccion
        self.siguiente_id += 1
        return transaccion

    def listar_por_usuario(self, usuario_id: int) -> list[TransaccionFake]:
        return sorted(
            [t for t in self.transacciones.values() if t.usuario_id == usuario_id],
            key=lambda t: (t.fecha, t.id),
            reverse=True,
        )

    def listar_por_usuario_orden_fecha(self, usuario_id: int) -> list[TransaccionFake]:
        return sorted(
            [t for t in self.transacciones.values() if t.usuario_id == usuario_id],
            key=lambda t: t.fecha,
        )

    def obtener_por_id_y_usuario(
        self,
        transaccion_id: int,
        usuario_id: int,
    ) -> TransaccionFake | None:
        transaccion = self.transacciones.get(transaccion_id)
        if transaccion is None or transaccion.usuario_id != usuario_id:
            return None
        return transaccion

    def actualizar(self, transaccion: TransaccionFake, cambios: dict[str, Any]) -> TransaccionFake:
        for campo, valor in cambios.items():
            setattr(transaccion, campo, valor)
        return transaccion

    def eliminar(self, transaccion: TransaccionFake) -> None:
        self.transacciones.pop(transaccion.id, None)


class PasswordHasherFake:
    def hash(self, password: str) -> str:
        return f"hashed:{password}"

    def verificar(self, password_plano: str, password_hash: str) -> bool:
        return password_hash == self.hash(password_plano)


class TokenServiceFake:
    def crear_access_token(self, subject: str) -> str:
        return f"token:{subject}"

    def decodificar_access_token(self, token: str) -> str | None:
        if not token.startswith("token:"):
            return None
        return token.removeprefix("token:")
