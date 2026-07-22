from datetime import date

import pytest

from app.application.errors import TransaccionNoEncontradaError
from app.application.schemas import TransaccionCreate, TransaccionUpdate
from app.application.use_cases.actualizar_transaccion import actualizar_transaccion
from app.application.use_cases.crear_transaccion import crear_transaccion
from app.application.use_cases.eliminar_transaccion import eliminar_transaccion
from app.application.use_cases.listar_transacciones import listar_transacciones
from app.application.use_cases.obtener_resumen_presupuesto import obtener_resumen_presupuesto
from app.application.use_cases.obtener_transaccion import obtener_transaccion
from app.domain.entities import TipoTransaccion

from tests.application.fakes import TransaccionRepositoryFake


def test_crear_y_obtener_transaccion():
    transacciones = TransaccionRepositoryFake()

    creada = crear_transaccion(
        TransaccionCreate(
            monto=10.5,
            descripcion="Almuerzo",
            categoria="Comida",
            fecha=date(2026, 7, 14),
            tipo=TipoTransaccion.gasto,
        ),
        transacciones,
        usuario_id=1,
    )
    obtenida = obtener_transaccion(transacciones, creada.id, usuario_id=1)

    assert obtenida.descripcion == "Almuerzo"
    assert obtenida.usuario_id == 1


def test_obtener_transaccion_rechaza_acceso_a_otro_usuario():
    transacciones = TransaccionRepositoryFake()
    creada = crear_transaccion(
        TransaccionCreate(
            monto=10.5,
            descripcion="Almuerzo",
            categoria="Comida",
            fecha=date(2026, 7, 14),
            tipo=TipoTransaccion.gasto,
        ),
        transacciones,
        usuario_id=1,
    )

    with pytest.raises(TransaccionNoEncontradaError):
        obtener_transaccion(transacciones, creada.id, usuario_id=2)


def test_listar_actualizar_y_eliminar_transacciones():
    transacciones = TransaccionRepositoryFake()
    primera = crear_transaccion(
        TransaccionCreate(
            monto=100,
            descripcion="Pago",
            categoria="Trabajo",
            fecha=date(2026, 7, 14),
            tipo=TipoTransaccion.ingreso,
        ),
        transacciones,
        usuario_id=1,
    )
    crear_transaccion(
        TransaccionCreate(
            monto=5,
            descripcion="Cafe",
            categoria="Comida",
            fecha=date(2026, 7, 15),
            tipo=TipoTransaccion.gasto,
        ),
        transacciones,
        usuario_id=1,
    )

    lista = listar_transacciones(transacciones, usuario_id=1)
    assert [t.descripcion for t in lista] == ["Cafe", "Pago"]

    actualizada = actualizar_transaccion(
        primera.id,
        TransaccionUpdate(monto=80, descripcion="Pago ajustado"),
        transacciones,
        usuario_id=1,
    )
    assert actualizada.monto == 80
    assert actualizada.descripcion == "Pago ajustado"

    eliminar_transaccion(transacciones, primera.id, usuario_id=1)
    with pytest.raises(TransaccionNoEncontradaError):
        obtener_transaccion(transacciones, primera.id, usuario_id=1)


def test_obtener_resumen_presupuesto_calcula_ingresos_gastos_y_saldo():
    transacciones = TransaccionRepositoryFake()
    crear_transaccion(
        TransaccionCreate(
            monto=100,
            descripcion="Pago",
            categoria="Trabajo",
            fecha=date(2026, 7, 14),
            tipo=TipoTransaccion.ingreso,
        ),
        transacciones,
        usuario_id=1,
    )
    crear_transaccion(
        TransaccionCreate(
            monto=25,
            descripcion="Comida",
            categoria="Comida",
            fecha=date(2026, 7, 14),
            tipo=TipoTransaccion.gasto,
        ),
        transacciones,
        usuario_id=1,
    )

    resumen = obtener_resumen_presupuesto(transacciones, usuario_id=1)

    assert resumen.total_ingresos == 100
    assert resumen.total_gastos == 25
    assert resumen.saldo_actual == 75
    assert resumen.resumen_diario[0].cantidad_transacciones == 2
