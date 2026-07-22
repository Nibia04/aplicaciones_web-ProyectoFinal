from datetime import date

from app.domain.entities import TipoTransaccion, Transaccion, Usuario


def test_transaccion_es_entidad_pura_sin_orm():
    transaccion = Transaccion(
        id=1,
        monto=25.5,
        descripcion="Almuerzo",
        categoria="Comida",
        fecha=date(2026, 7, 14),
        tipo=TipoTransaccion.gasto,
        usuario_id=10,
    )

    assert transaccion.id == 1
    assert transaccion.tipo == TipoTransaccion.gasto
    assert transaccion.usuario_id == 10
    assert not hasattr(transaccion, "__table__")


def test_usuario_es_entidad_pura_sin_orm():
    usuario = Usuario(
        id=7,
        nombre="Ana",
        email="ana@example.com",
        password_hash="hash",
    )

    assert usuario.email == "ana@example.com"
    assert not hasattr(usuario, "__table__")


def test_tipo_transaccion_expone_lenguaje_de_dominio():
    assert TipoTransaccion.ingreso.value == "ingreso"
    assert TipoTransaccion.gasto.value == "gasto"
