def test_crud_transaccion(client, auth_headers):
    creada = client.post(
        "/transacciones",
        headers=auth_headers,
        json={
            "monto": 10.5,
            "descripcion": "Almuerzo",
            "categoria": "Comida",
            "fecha": "2026-07-14",
            "tipo": "gasto",
        },
    )
    assert creada.status_code == 201
    transaccion_id = creada.json()["id"]

    listado = client.get("/transacciones", headers=auth_headers)
    assert listado.status_code == 200
    assert len(listado.json()) == 1

    obtenida = client.get(f"/transacciones/{transaccion_id}", headers=auth_headers)
    assert obtenida.status_code == 200
    assert obtenida.json()["descripcion"] == "Almuerzo"

    actualizada = client.put(
        f"/transacciones/{transaccion_id}",
        headers=auth_headers,
        json={"monto": 12.0, "descripcion": "Almuerzo con bebida"},
    )
    assert actualizada.status_code == 200
    assert actualizada.json()["monto"] == 12.0

    eliminada = client.delete(f"/transacciones/{transaccion_id}", headers=auth_headers)
    assert eliminada.status_code == 204

    no_encontrada = client.get(f"/transacciones/{transaccion_id}", headers=auth_headers)
    assert no_encontrada.status_code == 404
    assert no_encontrada.json()["error"]["codigo"] == "transaccion_no_encontrada"


def test_transaccion_requiere_autenticacion(client):
    response = client.get("/transacciones")

    assert response.status_code == 401
    assert response.json()["error"]["codigo"] == "http_401"


def test_crear_transaccion_valida_datos(client, auth_headers):
    response = client.post(
        "/transacciones",
        headers=auth_headers,
        json={
            "monto": -10,
            "descripcion": "",
            "categoria": "",
            "fecha": "2026-07-14",
            "tipo": "gasto",
        },
    )

    assert response.status_code == 422
    assert response.json()["error"]["codigo"] == "datos_invalidos"
