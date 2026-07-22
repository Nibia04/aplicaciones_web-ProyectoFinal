import pytest
from fastapi.testclient import TestClient
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker

from app.infrastructure.database import Base, get_db
from app.main import app


@pytest.fixture
def client(tmp_path):
    db_path = tmp_path / "api_test.db"
    engine = create_engine(
        f"sqlite:///{db_path}",
        connect_args={"check_same_thread": False},
    )
    TestingSessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)
    Base.metadata.create_all(bind=engine)

    def override_get_db():
        db = TestingSessionLocal()
        try:
            yield db
        finally:
            db.close()

    previous_overrides = dict(app.dependency_overrides)
    app.dependency_overrides[get_db] = override_get_db
    try:
        yield TestClient(app)
    finally:
        app.dependency_overrides.clear()
        app.dependency_overrides.update(previous_overrides)
        Base.metadata.drop_all(bind=engine)


@pytest.fixture
def usuario_payload():
    return {
        "nombre": "Ana",
        "email": "ana@example.com",
        "password": "secreto123",
    }


@pytest.fixture
def auth_headers(client, usuario_payload):
    client.post("/auth/registro", json=usuario_payload)
    response = client.post(
        "/auth/login",
        json={
            "email": usuario_payload["email"],
            "password": usuario_payload["password"],
        },
    )
    token = response.json()["access_token"]
    return {"Authorization": f"Bearer {token}"}
