from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
from fastapi.testclient import TestClient

from app.infrastructure.database import Base, get_db
from app.main import app


SQLALCHEMY_DATABASE_URL = "sqlite:///./test_presupuesto.db"

engine = create_engine(
    SQLALCHEMY_DATABASE_URL,
    connect_args={"check_same_thread": False},
)
TestingSessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)


def override_get_db():
    db = TestingSessionLocal()
    try:
        yield db
    finally:
        db.close()


app.dependency_overrides[get_db] = override_get_db
client = TestClient(app)


def setup_function():
    Base.metadata.drop_all(bind=engine)
    Base.metadata.create_all(bind=engine)


def test_health_check():
    response = client.get("/health")

    assert response.status_code == 200
    assert response.json() == {"status": "ok"}


def test_registro_con_email_duplicado():
    usuario = {
        "nombre": "Carlos",
        "email": "carlos@example.com",
        "password": "clave123",
    }

    primer_registro = client.post("/auth/registro", json=usuario)
    segundo_registro = client.post("/auth/registro", json=usuario)

    assert primer_registro.status_code == 201
    assert segundo_registro.status_code == 409
    assert segundo_registro.json() == {"detail": "El email ya esta registrado"}


def test_login_con_password_incorrecto():
    client.post(
        "/auth/registro",
        json={
            "nombre": "Maria",
            "email": "maria@example.com",
            "password": "clave456",
        },
    )

    response = client.post(
        "/auth/login",
        json={
            "email": "maria@example.com",
            "password": "password-incorrecto",
        },
    )

    assert response.status_code == 401
    assert response.json() == {"detail": "Credenciales invalidas"}


def test_registro_login_crear_transaccion_y_resumen():
    registro_response = client.post(
        "/auth/registro",
        json={
            "nombre": "Ana",
            "email": "ana@example.com",
            "password": "secreto123",
        },
    )

    assert registro_response.status_code == 201
    assert registro_response.json()["email"] == "ana@example.com"

    login_response = client.post(
        "/auth/login",
        json={
            "email": "ana@example.com",
            "password": "secreto123",
        },
    )

    assert login_response.status_code == 200
    token = login_response.json()["access_token"]
    headers = {"Authorization": f"Bearer {token}"}

    transaccion_response = client.post(
        "/transacciones",
        headers=headers,
        json={
            "monto": 10.5,
            "descripcion": "Almuerzo",
            "categoria": "Comida",
            "fecha": "2026-07-14",
            "tipo": "gasto",
        },
    )

    assert transaccion_response.status_code == 201
    assert transaccion_response.json()["descripcion"] == "Almuerzo"

    resumen_response = client.get("/presupuesto/resumen", headers=headers)

    assert resumen_response.status_code == 200
    resumen = resumen_response.json()
    assert resumen["saldo_actual"] == -10.5
    assert resumen["total_ingresos"] == 0
    assert resumen["total_gastos"] == 10.5
    assert resumen["resumen_diario"][0]["cantidad_transacciones"] == 1
