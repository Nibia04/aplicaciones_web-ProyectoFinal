from sqlalchemy import select
from sqlalchemy.orm import Session

from app.application.schemas import PresupuestoOut, ResumenDiarioOut
from app.domain.models import TipoTransaccion
from app.infrastructure.orm_models import Transaccion


def construir_resumen_presupuesto(db: Session, usuario_id: int) -> PresupuestoOut:
    transacciones = db.scalars(
        select(Transaccion)
        .where(Transaccion.usuario_id == usuario_id)
        .order_by(Transaccion.fecha.asc())
    ).all()

    total_ingresos = sum(t.monto for t in transacciones if t.tipo == TipoTransaccion.ingreso)
    total_gastos = sum(t.monto for t in transacciones if t.tipo == TipoTransaccion.gasto)

    resumen_por_fecha: dict = {}
    for transaccion in transacciones:
        datos = resumen_por_fecha.setdefault(
            transaccion.fecha,
            {"ingresos": 0.0, "gastos": 0.0, "cantidad": 0},
        )
        if transaccion.tipo == TipoTransaccion.ingreso:
            datos["ingresos"] += transaccion.monto
        else:
            datos["gastos"] += transaccion.monto
        datos["cantidad"] += 1

    resumen_diario = [
        ResumenDiarioOut(
            fecha=fecha,
            ingresos=datos["ingresos"],
            gastos=datos["gastos"],
            saldo=datos["ingresos"] - datos["gastos"],
            cantidad_transacciones=datos["cantidad"],
        )
        for fecha, datos in resumen_por_fecha.items()
    ]

    return PresupuestoOut(
        saldo_actual=total_ingresos - total_gastos,
        total_ingresos=total_ingresos,
        total_gastos=total_gastos,
        resumen_diario=resumen_diario,
    )
