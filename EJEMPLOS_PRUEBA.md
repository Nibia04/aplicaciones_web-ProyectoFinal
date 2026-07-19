# Ejemplos de Prueba - Gestor de Presupuesto Diario

## 📋 Descripción de la Aplicación

API FastAPI para gestionar **ingresos y gastos** diarios. Los usuarios pueden:
- ✅ Registrarse e iniciar sesión
- ✅ Crear transacciones (ingresos/gastos)
- ✅ Consultar y actualizar transacciones
- ✅ Ver resumen de presupuesto diario

**Tipos de transacción:** `ingreso` | `gasto`

---

## 🚀 Endpoints Principales

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| POST | `/auth/registro` | Registrar usuario |
| POST | `/auth/login` | Iniciar sesión (obtener token) |
| POST | `/transacciones` | Crear transacción |
| GET | `/transacciones` | Listar todas las transacciones |
| GET | `/transacciones/{id}` | Obtener una transacción |
| PUT | `/transacciones/{id}` | Actualizar transacción |
| DELETE | `/transacciones/{id}` | Eliminar transacción |
| GET | `/presupuesto/resumen` | Ver resumen de presupuesto |
| GET | `/health` | Verificar estado de la API |

---

## 💻 Ejemplos con cURL

### 1️⃣ Registrar un Usuario

```bash
curl -X POST "http://localhost:8000/auth/registro" \
  -H "Content-Type: application/json" \
  -d '{
    "nombre": "Juan Pérez",
    "email": "juan@example.com",
    "password": "password123"
  }'
```

**Respuesta esperada (201):**
```json
{
  "id": 1,
  "nombre": "Juan Pérez",
  "email": "juan@example.com"
}
```

---

### 2️⃣ Iniciar Sesión

```bash
curl -X POST "http://localhost:8000/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "juan@example.com",
    "password": "password123"
  }'
```

**Respuesta esperada (200):**
```json
{
  "access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "token_type": "bearer"
}
```

💾 **Guardar el token** para usar en los siguientes requests:
```bash
TOKEN="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

---

### 3️⃣ Crear una Transacción de Ingreso

```bash
curl -X POST "http://localhost:8000/transacciones" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "monto": 2500.00,
    "descripcion": "Salario de quincena",
    "categoria": "Salario",
    "fecha": "2026-07-19",
    "tipo": "ingreso"
  }'
```

**Respuesta esperada (201):**
```json
{
  "id": 1,
  "monto": 2500.0,
  "descripcion": "Salario de quincena",
  "categoria": "Salario",
  "fecha": "2026-07-19",
  "tipo": "ingreso"
}
```

---

### 4️⃣ Crear Transacciones de Gasto

```bash
# Gasto en comida
curl -X POST "http://localhost:8000/transacciones" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "monto": 45.50,
    "descripcion": "Almuerzo en restaurante",
    "categoria": "Comida",
    "fecha": "2026-07-19",
    "tipo": "gasto"
  }'

# Gasto en transporte
curl -X POST "http://localhost:8000/transacciones" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "monto": 20.00,
    "descripcion": "Gasolina",
    "categoria": "Transporte",
    "fecha": "2026-07-19",
    "tipo": "gasto"
  }'

# Gasto en servicios
curl -X POST "http://localhost:8000/transacciones" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "monto": 60.00,
    "descripcion": "Pago de internet",
    "categoria": "Servicios",
    "fecha": "2026-07-19",
    "tipo": "gasto"
  }'
```

---

### 5️⃣ Listar todas las Transacciones

```bash
curl -X GET "http://localhost:8000/transacciones" \
  -H "Authorization: Bearer $TOKEN"
```

**Respuesta esperada (200):**
```json
[
  {
    "id": 1,
    "monto": 2500.0,
    "descripcion": "Salario de quincena",
    "categoria": "Salario",
    "fecha": "2026-07-19",
    "tipo": "ingreso"
  },
  {
    "id": 2,
    "monto": 45.5,
    "descripcion": "Almuerzo en restaurante",
    "categoria": "Comida",
    "fecha": "2026-07-19",
    "tipo": "gasto"
  },
  ...
]
```

---

### 6️⃣ Obtener una Transacción Específica

```bash
curl -X GET "http://localhost:8000/transacciones/1" \
  -H "Authorization: Bearer $TOKEN"
```

**Respuesta esperada (200):**
```json
{
  "id": 1,
  "monto": 2500.0,
  "descripcion": "Salario de quincena",
  "categoria": "Salario",
  "fecha": "2026-07-19",
  "tipo": "ingreso"
}
```

---

### 7️⃣ Actualizar una Transacción

```bash
curl -X PUT "http://localhost:8000/transacciones/2" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "monto": 50.00,
    "descripcion": "Almuerzo en restaurante (aumentado)"
  }'
```

**Respuesta esperada (200):**
```json
{
  "id": 2,
  "monto": 50.0,
  "descripcion": "Almuerzo en restaurante (aumentado)",
  "categoria": "Comida",
  "fecha": "2026-07-19",
  "tipo": "gasto"
}
```

---

### 8️⃣ Eliminar una Transacción

```bash
curl -X DELETE "http://localhost:8000/transacciones/3" \
  -H "Authorization: Bearer $TOKEN"
```

**Respuesta esperada (204):** Sin contenido

---

### 9️⃣ Ver Resumen de Presupuesto

```bash
curl -X GET "http://localhost:8000/presupuesto/resumen" \
  -H "Authorization: Bearer $TOKEN"
```

**Respuesta esperada (200):**
```json
{
  "saldo_actual": 2374.5,
  "total_ingresos": 2500.0,
  "total_gastos": 125.5,
  "resumen_diario": [
    {
      "fecha": "2026-07-19",
      "ingresos": 2500.0,
      "gastos": 125.5,
      "saldo": 2374.5,
      "cantidad_transacciones": 3
    }
  ]
}
```

---

## 🧪 Pruebas de Validación

### ❌ Registrar con email duplicado

```bash
curl -X POST "http://localhost:8000/auth/registro" \
  -H "Content-Type: application/json" \
  -d '{
    "nombre": "Otro Usuario",
    "email": "juan@example.com",
    "password": "password123"
  }'
```

**Respuesta esperada (409):**
```json
{
  "detail": "El email ya esta registrado"
}
```

---

### ❌ Login con credenciales incorrectas

```bash
curl -X POST "http://localhost:8000/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "juan@example.com",
    "password": "passwordIncorrecto"
  }'
```

**Respuesta esperada (401):**
```json
{
  "detail": "Credenciales invalidas"
}
```

---

### ❌ Acceder a transacción sin autenticación

```bash
curl -X GET "http://localhost:8000/transacciones"
```

**Respuesta esperada (403):** 
```json
{
  "detail": "Not authenticated"
}
```

---

### ❌ Crear transacción con monto negativo

```bash
curl -X POST "http://localhost:8000/transacciones" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "monto": -50.00,
    "descripcion": "Transacción inválida",
    "categoria": "Prueba",
    "fecha": "2026-07-19",
    "tipo": "gasto"
  }'
```

**Respuesta esperada (422):** Validation error

---

### ❌ Obtener transacción que no existe

```bash
curl -X GET "http://localhost:8000/transacciones/999" \
  -H "Authorization: Bearer $TOKEN"
```

**Respuesta esperada (404):**
```json
{
  "detail": "Transaccion no encontrada"
}
```

---

## 📝 Script de Prueba Completo en Bash

Crea un archivo `prueba_api.sh`:

```bash
#!/bin/bash

# Colores para output
GREEN='\033[0;32m'
RED='\033[0;31m'
NC='\033[0m' # No Color

BASE_URL="http://localhost:8000"

echo "=== 1. Registrar Usuario ==="
REGISTRO=$(curl -s -X POST "$BASE_URL/auth/registro" \
  -H "Content-Type: application/json" \
  -d '{
    "nombre": "María García",
    "email": "maria@example.com",
    "password": "secure123"
  }')
echo $REGISTRO | jq '.'

echo -e "\n=== 2. Iniciar Sesión ==="
LOGIN=$(curl -s -X POST "$BASE_URL/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "maria@example.com",
    "password": "secure123"
  }')
echo $LOGIN | jq '.'

TOKEN=$(echo $LOGIN | jq -r '.access_token')
echo -e "${GREEN}✓ Token obtenido${NC}"

echo -e "\n=== 3. Crear Ingreso ==="
curl -s -X POST "$BASE_URL/transacciones" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "monto": 3000,
    "descripcion": "Pago freelance",
    "categoria": "Ingresos Extras",
    "fecha": "2026-07-19",
    "tipo": "ingreso"
  }' | jq '.'

echo -e "\n=== 4. Crear Gastos ==="
curl -s -X POST "$BASE_URL/transacciones" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "monto": 30,
    "descripcion": "Café y desayuno",
    "categoria": "Comida",
    "fecha": "2026-07-19",
    "tipo": "gasto"
  }' | jq '.'

curl -s -X POST "$BASE_URL/transacciones" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "monto": 150,
    "descripcion": "Compra en supermercado",
    "categoria": "Comida",
    "fecha": "2026-07-19",
    "tipo": "gasto"
  }' | jq '.'

echo -e "\n=== 5. Listar Transacciones ==="
curl -s -X GET "$BASE_URL/transacciones" \
  -H "Authorization: Bearer $TOKEN" | jq '.'

echo -e "\n=== 6. Ver Resumen de Presupuesto ==="
curl -s -X GET "$BASE_URL/presupuesto/resumen" \
  -H "Authorization: Bearer $TOKEN" | jq '.'

echo -e "\n${GREEN}✓ Pruebas completadas${NC}"
```

Ejecuta:
```bash
chmod +x prueba_api.sh
./prueba_api.sh
```

---

## 🔧 Requisitos para Ejecutar

1. **Asegúrate de que el servidor está corriendo:**
```bash
python -m uvicorn app.main:app --reload
```

2. **Instala `jq` (opcional, para mejor formato de JSON):**
   - Windows: `choco install jq`
   - Linux: `sudo apt-get install jq`
   - macOS: `brew install jq`

---

## 📊 Flujo Recomendado de Prueba

1. ✅ Verificar salud: `GET /health`
2. ✅ Registrar usuario
3. ✅ Iniciar sesión (obtener token)
4. ✅ Crear ingresos
5. ✅ Crear gastos
6. ✅ Listar transacciones
7. ✅ Actualizar una transacción
8. ✅ Ver resumen
9. ✅ Eliminar una transacción
10. ✅ Verificar resumen actualizado

---

## 🐛 Troubleshooting

| Error | Solución |
|-------|----------|
| `Connection refused` | Asegúrate de que la API está corriendo en puerto 8000 |
| `"Not authenticated"` | Verifica que el token JWT sea válido y esté en el header `Authorization: Bearer` |
| `"Credenciales invalidas"` | Revisa email y password sean correctos |
| `"Transaccion no encontrada"` | El ID de la transacción no existe o fue eliminada |
| `Validation error` | Verifica que los datos cumplan los requisitos (monto > 0, email válido, etc.) |

