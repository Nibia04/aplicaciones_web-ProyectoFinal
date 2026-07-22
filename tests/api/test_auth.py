def test_registro_usuario(client, usuario_payload):
    response = client.post("/auth/registro", json=usuario_payload)

    assert response.status_code == 201
    assert response.json()["email"] == usuario_payload["email"]
    assert "password" not in response.json()


def test_registro_rechaza_email_duplicado(client, usuario_payload):
    client.post("/auth/registro", json=usuario_payload)

    response = client.post("/auth/registro", json=usuario_payload)

    assert response.status_code == 409
    assert response.json()["error"]["codigo"] == "email_ya_registrado"


def test_login_usuario(client, usuario_payload):
    client.post("/auth/registro", json=usuario_payload)

    response = client.post(
        "/auth/login",
        json={
            "email": usuario_payload["email"],
            "password": usuario_payload["password"],
        },
    )

    assert response.status_code == 200
    assert response.json()["token_type"] == "bearer"
    assert response.json()["access_token"]


def test_login_rechaza_password_incorrecto(client, usuario_payload):
    client.post("/auth/registro", json=usuario_payload)

    response = client.post(
        "/auth/login",
        json={
            "email": usuario_payload["email"],
            "password": "incorrecto",
        },
    )

    assert response.status_code == 401
    assert response.json()["error"]["codigo"] == "credenciales_invalidas"
