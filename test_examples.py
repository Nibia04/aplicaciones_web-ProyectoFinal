#!/usr/bin/env python3
"""
Script para probar la API de Gestor de Presupuesto Diario
Uso: python test_examples.py
"""

import requests
import json
from datetime import date

BASE_URL = "http://localhost:8000"

# Colores para terminal
class Colors:
    GREEN = '\033[92m'
    RED = '\033[91m'
    YELLOW = '\033[93m'
    BLUE = '\033[94m'
    END = '\033[0m'

def print_header(text):
    print(f"\n{Colors.BLUE}{'='*60}")
    print(f"{text}")
    print(f"{'='*60}{Colors.END}\n")

def print_success(text):
    print(f"{Colors.GREEN}✓ {text}{Colors.END}")

def print_error(text):
    print(f"{Colors.RED}✗ {text}{Colors.END}")

def print_response(response):
    try:
        print(json.dumps(response.json(), indent=2, ensure_ascii=False))
    except:
        print(response.text)

def test_health():
    """Verificar que la API está disponible"""
    print_header("1. Verificar Health Check")
    try:
        response = requests.get(f"{BASE_URL}/health")
        if response.status_code == 200:
            print_success("API está disponible")
            print_response(response)
        else:
            print_error(f"Error: {response.status_code}")
    except Exception as e:
        print_error(f"No se pudo conectar: {e}")
        return False
    return True

def test_registration():
    """Registrar un nuevo usuario"""
    print_header("2. Registrar Usuario")
    payload = {
        "nombre": "Carlos López",
        "email": "carlos@example.com",
        "password": "securepass123"
    }
    response = requests.post(f"{BASE_URL}/auth/registro", json=payload)
    print(f"Status: {response.status_code}")
    print_response(response)

    if response.status_code == 201:
        print_success("Usuario registrado correctamente")
        return response.json()
    else:
        print_error("Error al registrar usuario")
        return None

def test_login(email: str, password: str):
    """Iniciar sesión y obtener token"""
    print_header("3. Iniciar Sesión")
    payload = {
        "email": email,
        "password": password
    }
    response = requests.post(f"{BASE_URL}/auth/login", json=payload)
    print(f"Status: {response.status_code}")
    print_response(response)

    if response.status_code == 200:
        token = response.json()["access_token"]
        print_success(f"Token obtenido: {token[:20]}...")
        return token
    else:
        print_error("Error al iniciar sesión")
        return None

def test_create_transaction(token: str, tipo: str, monto: float, descripcion: str, categoria: str):
    """Crear una transacción"""
    headers = {"Authorization": f"Bearer {token}"}
    payload = {
        "monto": monto,
        "descripcion": descripcion,
        "categoria": categoria,
        "fecha": str(date.today()),
        "tipo": tipo
    }
    response = requests.post(f"{BASE_URL}/transacciones", json=payload, headers=headers)
    print(f"Status: {response.status_code}")
    print_response(response)

    if response.status_code == 201:
        print_success(f"{tipo.capitalize()} registrado: ${monto}")
        return response.json()
    else:
        print_error(f"Error al crear {tipo}")
        return None

def test_list_transactions(token: str):
    """Listar todas las transacciones"""
    print_header("5. Listar Transacciones")
    headers = {"Authorization": f"Bearer {token}"}
    response = requests.get(f"{BASE_URL}/transacciones", headers=headers)
    print(f"Status: {response.status_code}")
    print_response(response)

    if response.status_code == 200:
        transacciones = response.json()
        print_success(f"Total de transacciones: {len(transacciones)}")
        return transacciones
    else:
        print_error("Error al listar transacciones")
        return []

def test_get_transaction(token: str, transaction_id: int):
    """Obtener una transacción específica"""
    print_header(f"6. Obtener Transacción ID: {transaction_id}")
    headers = {"Authorization": f"Bearer {token}"}
    response = requests.get(f"{BASE_URL}/transacciones/{transaction_id}", headers=headers)
    print(f"Status: {response.status_code}")
    print_response(response)

def test_update_transaction(token: str, transaction_id: int):
    """Actualizar una transacción"""
    print_header(f"7. Actualizar Transacción ID: {transaction_id}")
    headers = {"Authorization": f"Bearer {token}"}
    payload = {
        "monto": 75.00,
        "descripcion": "Almuerzo actualizado con propina"
    }
    response = requests.put(f"{BASE_URL}/transacciones/{transaction_id}", json=payload, headers=headers)
    print(f"Status: {response.status_code}")
    print_response(response)

def test_budget_summary(token: str):
    """Obtener resumen de presupuesto"""
    print_header("8. Resumen de Presupuesto")
    headers = {"Authorization": f"Bearer {token}"}
    response = requests.get(f"{BASE_URL}/presupuesto/resumen", headers=headers)
    print(f"Status: {response.status_code}")
    print_response(response)

    if response.status_code == 200:
        data = response.json()
        print_success(f"Saldo actual: ${data.get('saldo_actual', 0)}")
        print_success(f"Total ingresos: ${data.get('total_ingresos', 0)}")
        print_success(f"Total gastos: ${data.get('total_gastos', 0)}")

def test_delete_transaction(token: str, transaction_id: int):
    """Eliminar una transacción"""
    print_header(f"9. Eliminar Transacción ID: {transaction_id}")
    headers = {"Authorization": f"Bearer {token}"}
    response = requests.delete(f"{BASE_URL}/transacciones/{transaction_id}", headers=headers)
    print(f"Status: {response.status_code}")

    if response.status_code == 204:
        print_success("Transacción eliminada correctamente")
    else:
        print_error("Error al eliminar transacción")

def test_error_cases(token: str):
    """Pruebas de casos de error"""
    print_header("10. Pruebas de Errores")

    # Transacción con monto negativo
    print("\n--- Intento: Monto negativo ---")
    headers = {"Authorization": f"Bearer {token}"}
    payload = {
        "monto": -50,
        "descripcion": "Gasto inválido",
        "categoria": "Prueba",
        "fecha": str(date.today()),
        "tipo": "gasto"
    }
    response = requests.post(f"{BASE_URL}/transacciones", json=payload, headers=headers)
    print(f"Status: {response.status_code}")
    print_response(response)

    # ID de transacción que no existe
    print("\n--- Intento: Transacción inexistente ---")
    response = requests.get(f"{BASE_URL}/transacciones/9999", headers=headers)
    print(f"Status: {response.status_code}")
    print_response(response)

    # Sin autenticación
    print("\n--- Intento: Sin token de autenticación ---")
    response = requests.get(f"{BASE_URL}/transacciones")
    print(f"Status: {response.status_code}")
    print_response(response)

def main():
    """Ejecutar todas las pruebas"""
    print(f"\n{Colors.YELLOW}Iniciando pruebas de API de Presupuesto Diario{Colors.END}")

    # 1. Health check
    if not test_health():
        return

    # 2. Registrar usuario
    usuario = test_registration()
    if not usuario:
        return

    # 3. Login
    token = test_login(usuario["email"], "securepass123")
    if not token:
        return

    # 4. Crear transacciones
    print_header("4. Crear Transacciones")
    transaccion1 = test_create_transaction(token, "ingreso", 3000, "Salario mensual", "Salario")
    test_create_transaction(token, "gasto", 50, "Almuerzo", "Comida")
    test_create_transaction(token, "gasto", 25, "Taxi al trabajo", "Transporte")
    test_create_transaction(token, "gasto", 100, "Recarga de teléfono", "Servicios")

    # 5. Listar transacciones
    transacciones = test_list_transactions(token)

    # 6. Obtener transacción específica
    if transacciones:
        test_get_transaction(token, transacciones[0]["id"])

        # 7. Actualizar transacción
        test_update_transaction(token, transacciones[0]["id"])

    # 8. Resumen de presupuesto
    test_budget_summary(token)

    # 9. Eliminar transacción
    if len(transacciones) > 3:
        test_delete_transaction(token, transacciones[3]["id"])

    # 10. Casos de error
    test_error_cases(token)

    print(f"\n{Colors.GREEN}✓ Pruebas completadas{Colors.END}\n")

if __name__ == "__main__":
    try:
        main()
    except KeyboardInterrupt:
        print(f"\n{Colors.YELLOW}Pruebas interrumpidas por el usuario{Colors.END}")
    except Exception as e:
        print(f"\n{Colors.RED}Error inesperado: {e}{Colors.END}")
