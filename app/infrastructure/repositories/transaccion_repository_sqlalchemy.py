from datetime import date
from typing import Any

from sqlalchemy import select
from sqlalchemy.orm import Session

from app.domain.models import TipoTransaccion
from app.infrastructure.orm_models import Transaccion


class TransaccionRepositorySqlAlchemy:
    def __init__(self, db: Session) -> None:
        self.db = db

    def crear(
        self,
        monto: float,
        descripcion: str,
        categoria: str,
        fecha: date,
        tipo: TipoTransaccion,
        usuario_id: int,
    ) -> Transaccion:
        transaccion = Transaccion(
            monto=monto,
            descripcion=descripcion,
            categoria=categoria,
            fecha=fecha,
            tipo=tipo,
            usuario_id=usuario_id,
        )
        self.db.add(transaccion)
        self.db.commit()
        self.db.refresh(transaccion)
        return transaccion

    def listar_por_usuario(self, usuario_id: int) -> list[Transaccion]:
        return self.db.scalars(
            select(Transaccion)
            .where(Transaccion.usuario_id == usuario_id)
            .order_by(Transaccion.fecha.desc(), Transaccion.id.desc())
        ).all()

    def listar_por_usuario_orden_fecha(self, usuario_id: int) -> list[Transaccion]:
        return self.db.scalars(
            select(Transaccion)
            .where(Transaccion.usuario_id == usuario_id)
            .order_by(Transaccion.fecha.asc())
        ).all()

    def obtener_por_id_y_usuario(
        self,
        transaccion_id: int,
        usuario_id: int,
    ) -> Transaccion | None:
        return self.db.scalar(
            select(Transaccion).where(
                Transaccion.id == transaccion_id,
                Transaccion.usuario_id == usuario_id,
            )
        )

    def actualizar(self, transaccion: Transaccion, cambios: dict[str, Any]) -> Transaccion:
        for campo, valor in cambios.items():
            setattr(transaccion, campo, valor)

        self.db.commit()
        self.db.refresh(transaccion)
        return transaccion

    def eliminar(self, transaccion: Transaccion) -> None:
        self.db.delete(transaccion)
        self.db.commit()
