from datetime import date
from typing import Any, Protocol

from app.domain.models import TipoTransaccion


class UsuarioRepository(Protocol):
    def obtener_por_email(self, email: str) -> Any | None:
        ...

    def obtener_por_id(self, usuario_id: int) -> Any | None:
        ...

    def crear(self, nombre: str, email: str, password_hash: str) -> Any:
        ...


class TransaccionRepository(Protocol):
    def crear(
        self,
        monto: float,
        descripcion: str,
        categoria: str,
        fecha: date,
        tipo: TipoTransaccion,
        usuario_id: int,
    ) -> Any:
        ...

    def listar_por_usuario(self, usuario_id: int) -> list[Any]:
        ...

    def listar_por_usuario_orden_fecha(self, usuario_id: int) -> list[Any]:
        ...

    def obtener_por_id_y_usuario(self, transaccion_id: int, usuario_id: int) -> Any | None:
        ...

    def actualizar(self, transaccion: Any, cambios: dict[str, Any]) -> Any:
        ...

    def eliminar(self, transaccion: Any) -> None:
        ...


class PasswordHasher(Protocol):
    def hash(self, password: str) -> str:
        ...

    def verificar(self, password_plano: str, password_hash: str) -> bool:
        ...


class TokenService(Protocol):
    def crear_access_token(self, subject: str) -> str:
        ...

    def decodificar_access_token(self, token: str) -> str | None:
        ...
