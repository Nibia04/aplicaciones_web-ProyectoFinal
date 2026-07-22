def test_resumen_presupuesto(client, auth_headers):
    client.post(
        "/transacciones",
        headers=auth_headers,
        json={
            "monto": 100,
            "descripcion": "Pago",
            "categoria": "Trabajo",
            "fecha": "2026-07-14",
            "tipo": "ingreso",
        },
    )
    client.post(
        "/transacciones",
        headers=auth_headers,
        json={
            "monto": 25,
            "descripcion": "Almuerzo",
            "categoria": "Comida",
            "fecha": "2026-07-14",
            "tipo": "gasto",
        },
    )

    response = client.get("/presupuesto/resumen", headers=auth_headers)

    assert response.status_code == 200
    assert response.json()["total_ingresos"] == 100
    assert response.json()["total_gastos"] == 25
    assert response.json()["saldo_actual"] == 75
    assert response.json()["resumen_diario"][0]["cantidad_transacciones"] == 2
