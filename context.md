# Contexto del proyecto

## Proyecto actual

El proyecto `aplicaciones_web-ProyectoFinal` es un backend construido con **FastAPI**, **SQLite**, **SQLAlchemy**, **JWT** y **pytest**. Su objetivo principal es permitir que un usuario registre ingresos y gastos diarios, consulte sus transacciones y obtenga un resumen de presupuesto.

Actualmente el sistema tiene una organizacion parecida a una arquitectura por capas:

```text
app/
  main.py
  api/
    dependencies.py
    routes/
      auth.py
      transacciones.py
      presupuesto.py
  application/
    schemas.py
    services/
      presupuesto_service.py
  domain/
    models.py
  infrastructure/
    database.py
    security.py
tests/
  test_api.py
```

## Funcionalidades implementadas

El sistema actualmente permite:

- Registrar usuarios.
- Iniciar sesion con email y password.
- Generar token JWT.
- Crear transacciones de tipo ingreso o gasto.
- Listar transacciones del usuario autenticado.
- Consultar una transaccion por id.
- Actualizar una transaccion.
- Eliminar una transaccion.
- Consultar resumen de presupuesto por usuario.
- Revisar estado de la API con `/health`.

## Endpoints principales

```text
GET  /
GET  /health
POST /auth/registro
POST /auth/login
POST /transacciones
GET  /transacciones
GET  /transacciones/{transaccion_id}
PUT  /transacciones/{transaccion_id}
DELETE /transacciones/{transaccion_id}
GET  /presupuesto/resumen
```

## Estado actual de las capas

### API

La capa API existe y esta ubicada en:

```text
app/api/
```

Contiene las rutas HTTP de FastAPI. Actualmente maneja autenticacion, transacciones y presupuesto como una capa delgada sobre los casos de uso.

Archivos principales:

- `app/api/error_handlers.py`
- `app/api/routes/auth.py`
- `app/api/routes/transacciones.py`
- `app/api/routes/presupuesto.py`
- `app/api/dependencies.py`

Estado actual: la API es una capa delgada. Resuelve dependencias, llama casos de uso y delega la traduccion de errores a manejadores centralizados.

### Application

La capa de aplicacion existe en:

```text
app/application/
```

Actualmente contiene:

- `schemas.py`: DTOs y modelos de entrada/salida con Pydantic.
- `ports.py`: contratos de repositorios y servicios externos.
- `services/presupuesto_service.py`: servicio para construir el resumen de presupuesto.
- `errors.py`: errores esperados de aplicacion.
- `use_cases/`: casos de uso del sistema.

Esta capa ya contiene los casos de uso principales y depende de interfaces, no de SQLAlchemy.

### Domain

La capa de dominio existe en:

```text
app/domain/
```

Actualmente contiene:

- `entities.py`: entidades puras `Usuario`, `Transaccion` y enum `TipoTransaccion`.
- `models.py`: modulo de compatibilidad que reexporta los modelos de dominio.

El dominio ya no depende de SQLAlchemy ni de `Base`. Esta capa queda preparada para pruebas unitarias y reglas de negocio puras.

### Infrastructure

La capa de infraestructura existe en:

```text
app/infrastructure/
```

Actualmente contiene:

- `database.py`: configuracion de SQLite, SQLAlchemy, sesiones y `Base`.
- `orm_models.py`: modelos SQLAlchemy `UsuarioModel` y `TransaccionModel`.
- `repositories/usuario_repository_sqlalchemy.py`: repositorio concreto de usuarios.
- `repositories/transaccion_repository_sqlalchemy.py`: repositorio concreto de transacciones.
- `security.py`: hash de password, verificacion de password, creacion y decodificacion de JWT.

Esta capa contiene los detalles tecnicos y las implementaciones concretas de los puertos definidos en aplicacion.

### Testing

El proyecto tiene pruebas en:

```text
tests/test_api.py
```

Las pruebas actuales cubren:

- `GET /health`
- Flujo completo de registro, login, creacion de transaccion y resumen de presupuesto.

El testing existe, pero todavia es basico. Faltan pruebas unitarias del dominio, pruebas de casos de uso y mas pruebas especificas por endpoint.

## Diagnostico arquitectonico

El proyecto cumple parcialmente con una arquitectura por capas porque ya tiene separadas las carpetas:

- `api`
- `application`
- `domain`
- `infrastructure`
- `tests`

Sin embargo, todavia no cumple completamente con Clean Architecture o DDD porque:

- Las pruebas no cubren todas las capas.
- La configuracion sensible, como `SECRET_KEY`, esta escrita directamente en codigo.

## Casos de uso actuales

### 1. Registro e inicio de sesion

Permite registrar un usuario, validar credenciales y generar un token JWT.

Ubicacion actual:

```text
app/api/routes/auth.py
app/application/use_cases/registrar_usuario.py
app/application/use_cases/login_usuario.py
```

### 2. Gestion de transacciones

Permite crear, listar, obtener, actualizar y eliminar transacciones del usuario autenticado.

Ubicacion actual:

```text
app/api/routes/transacciones.py
app/application/use_cases/crear_transaccion.py
app/application/use_cases/listar_transacciones.py
app/application/use_cases/obtener_transaccion.py
app/application/use_cases/actualizar_transaccion.py
app/application/use_cases/eliminar_transaccion.py
```

### 3. Consulta de resumen de presupuesto

Permite calcular ingresos, gastos, saldo actual y resumen diario.

Ubicacion actual:

```text
app/api/routes/presupuesto.py
app/application/use_cases/obtener_resumen_presupuesto.py
app/application/services/presupuesto_service.py
```

## Objetivo de mejora

El objetivo es convertir el proyecto en una version mas clara y defendible de arquitectura limpia por capas, con separacion real entre:

```text
API -> Application -> Domain
        |
        v
 Infrastructure
```

La idea es que:

- La API solo reciba solicitudes HTTP y devuelva respuestas.
- Application contenga los casos de uso.
- Domain contenga entidades y reglas puras.
- Infrastructure contenga detalles tecnicos como base de datos, ORM, JWT y repositorios.
- Tests cubra dominio, aplicacion, API e infraestructura basica.

## Referencia de estructura esperada

La estructura objetivo debe tomar como referencia el ejemplo del proyecto `Abnb-main`, que separa claramente:

```text
src/
  Api/
  Aplicacion/
  Dominio/
  Infraestructura/
  Tests/
```

Como este proyecto esta hecho en Python con FastAPI, la equivalencia sera:

```text
app/
  api/              # equivalente a Api
  application/      # equivalente a Aplicacion
  domain/           # equivalente a Dominio
  infrastructure/   # equivalente a Infraestructura
tests/              # equivalente a Testing
```

La idea no es copiar C# literalmente, sino respetar el mismo criterio arquitectonico:

- `api`: entrada HTTP, controladores/rutas y dependencias web.
- `application`: casos de uso, comandos/consultas, DTOs y puertos.
- `domain`: entidades, value objects, reglas de negocio, errores y contratos centrales.
- `infrastructure`: ORM, base de datos, seguridad, JWT, repositorios concretos e integraciones.
- `tests`: pruebas automatizadas por capa.

Al avanzar las fases, se debe procurar que la organizacion final se parezca a esta forma:

```text
app/
  api/
    error_handlers.py
    dependencies.py
    routes/
      auth.py
      transacciones.py
      presupuesto.py
  application/
    schemas.py
    ports.py
    use_cases/
      registrar_usuario.py
      login_usuario.py
      crear_transaccion.py
      listar_transacciones.py
      obtener_transaccion.py
      actualizar_transaccion.py
      eliminar_transaccion.py
      obtener_resumen_presupuesto.py
  domain/
    entities.py
    errors.py
    repositories.py
    services.py
  infrastructure/
    database.py
    orm_models.py
    security.py
    repositories/
      usuario_repository_sqlalchemy.py
      transaccion_repository_sqlalchemy.py
tests/
  api/
  application/
  domain/
  infrastructure/
```

# Planificacion por fases


## Fase 3: Crear casos de uso en Application

Objetivo: mover la logica de negocio y aplicacion fuera de las rutas.

Estado: completada.

Acciones:

- Crear una carpeta de casos de uso:

```text
app/application/use_cases/
```

- Crear casos de uso como:

```text
registrar_usuario.py
login_usuario.py
crear_transaccion.py
listar_transacciones.py
obtener_transaccion.py
actualizar_transaccion.py
eliminar_transaccion.py
obtener_resumen_presupuesto.py
```

- Hacer que las rutas llamen a estos casos de uso en lugar de ejecutar toda la logica directamente.

- Crear errores de aplicacion en:

```text
app/application/errors.py
```

- Dejar la capa API encargada de traducir errores de aplicacion a respuestas HTTP.

Resultado esperado:

- La capa `application` contendra claramente los casos de uso del sistema.

## Fase 4: Crear repositorios e interfaces

Objetivo: desacoplar los casos de uso de SQLAlchemy.

Estado: completada.

Acciones:

- Crear contratos o interfaces para repositorios:

```text
app/application/ports.py
```

o:

```text
app/domain/repositories.py
```

- Definir contratos como:

```text
UsuarioRepository
TransaccionRepository
PasswordHasher
TokenService
```

- Crear implementaciones concretas con SQLAlchemy en infraestructura:

```text
app/infrastructure/repositories/
  usuario_repository_sqlalchemy.py
  transaccion_repository_sqlalchemy.py
```

- Actualizar los casos de uso para recibir repositorios y servicios por interfaz.
- Mantener SQLAlchemy solamente en infraestructura.
- Mantener la API como punto de composicion que crea los repositorios concretos.

Resultado esperado:

- Application dependera de interfaces.
- Infrastructure tendra las implementaciones concretas.
- Sera mas facil probar casos de uso con repositorios falsos o en memoria.

## Fase 5: Mejorar la API

Objetivo: dejar la API como una capa delgada.

Estado: completada.

Acciones:

- Mantener los endpoints existentes.
- Hacer que cada endpoint:
  - reciba el request,
  - valide entrada mediante schemas,
  - llame al caso de uso correspondiente,
  - transforme errores en respuestas HTTP,
  - devuelva la respuesta.

- Evitar consultas SQLAlchemy directamente dentro de rutas.
- Estandarizar errores y respuestas.
- Centralizar la traduccion de errores en:

```text
app/api/error_handlers.py
```

- Centralizar la creacion de repositorios y servicios en:

```text
app/api/dependencies.py
```

- Usar un formato consistente para errores:

```json
{
  "error": {
    "codigo": "codigo_del_error",
    "mensaje": "Descripcion del error"
  }
}
```

Resultado esperado:

- La API sera mas limpia, facil de leer y alineada con arquitectura limpia.

## Fase 6: Mejorar testing

Objetivo: cubrir las capas principales del proyecto.

Acciones:

- Mantener las pruebas actuales de API.
- Crear pruebas de dominio:

```text
tests/domain/
  test_transaccion.py
```

- Crear pruebas de aplicacion:

```text
tests/application/
  test_crear_transaccion.py
  test_obtener_resumen_presupuesto.py
```

- Crear pruebas de API por modulo:

```text
tests/api/
  test_auth.py
  test_transacciones.py
  test_presupuesto.py
```

- Verificar casos importantes:
  - registro exitoso,
  - email duplicado,
  - login correcto,
  - login incorrecto,
  - crear ingreso,
  - crear gasto,
  - listar transacciones propias,
  - no acceder a transacciones de otro usuario,
  - actualizar transaccion,
  - eliminar transaccion,
  - calcular resumen correcto.

Resultado esperado:

- El proyecto tendra testing por capas y no solo una prueba de flujo completo.

## Fase 7: Mejorar seguridad y configuracion

Objetivo: evitar configuraciones sensibles escritas directamente en codigo.

Acciones:

- Crear archivo de configuracion:

```text
app/infrastructure/config.py
```

- Mover valores configurables a variables de entorno:
  - `SECRET_KEY`
  - `DATABASE_URL`
  - `ACCESS_TOKEN_EXPIRE_MINUTES`

- Evitar claves fijas como:

```text
SECRET_KEY = "cambia-esta-clave-en-produccion"
```

Resultado esperado:

- El proyecto sera mas seguro y mas facil de configurar en distintos entornos.

## Fase 8: Actualizar documentacion final

Objetivo: que el proyecto pueda defenderse facilmente ante una revision academica.

Acciones:

- Actualizar `README.md` con:
  - descripcion del proyecto,
  - arquitectura usada,
  - explicacion de capas,
  - casos de uso,
  - endpoints,
  - instrucciones para ejecutar,
  - instrucciones para correr tests.

- Incluir un diagrama simple:

```text
API -> Application -> Domain
        |
        v
 Infrastructure
```

Resultado esperado:

- Cualquier integrante o profesor podra entender rapidamente la arquitectura y el estado del sistema.

## Prioridad recomendada

El orden recomendado para trabajar es:

1. Separar dominio de SQLAlchemy.
2. Mover logica de rutas a casos de uso.
3. Crear repositorios e interfaces.
4. Mejorar pruebas.
5. Mover configuracion sensible a variables de entorno.
6. Actualizar README y documentacion final.
