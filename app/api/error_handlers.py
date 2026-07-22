from fastapi import FastAPI, HTTPException, Request, status
from fastapi.encoders import jsonable_encoder
from fastapi.exceptions import RequestValidationError
from fastapi.responses import JSONResponse

from app.application.errors import (
    ApplicationError,
    CredencialesInvalidasError,
    EmailYaRegistradoError,
    TransaccionNoEncontradaError,
)


ERROR_STATUS = {
    EmailYaRegistradoError: (status.HTTP_409_CONFLICT, "email_ya_registrado"),
    CredencialesInvalidasError: (status.HTTP_401_UNAUTHORIZED, "credenciales_invalidas"),
    TransaccionNoEncontradaError: (status.HTTP_404_NOT_FOUND, "transaccion_no_encontrada"),
}


def construir_error(codigo: str, mensaje: str, detalles=None) -> dict:
    contenido = {"error": {"codigo": codigo, "mensaje": mensaje}}
    if detalles is not None:
        contenido["error"]["detalles"] = detalles
    return contenido


async def manejar_error_aplicacion(
    request: Request,
    exc: ApplicationError,
) -> JSONResponse:
    status_code, codigo = ERROR_STATUS.get(
        type(exc),
        (status.HTTP_400_BAD_REQUEST, "error_aplicacion"),
    )
    return JSONResponse(
        status_code=status_code,
        content=construir_error(codigo=codigo, mensaje=str(exc)),
    )


async def manejar_http_exception(
    request: Request,
    exc: HTTPException,
) -> JSONResponse:
    mensaje = exc.detail if isinstance(exc.detail, str) else "Error HTTP"
    return JSONResponse(
        status_code=exc.status_code,
        headers=exc.headers,
        content=construir_error(
            codigo=f"http_{exc.status_code}",
            mensaje=mensaje,
            detalles=None if isinstance(exc.detail, str) else jsonable_encoder(exc.detail),
        ),
    )


async def manejar_error_validacion(
    request: Request,
    exc: RequestValidationError,
) -> JSONResponse:
    return JSONResponse(
        status_code=status.HTTP_422_UNPROCESSABLE_ENTITY,
        content=construir_error(
            codigo="datos_invalidos",
            mensaje="Datos de entrada invalidos",
            detalles=jsonable_encoder(exc.errors()),
        ),
    )


def registrar_manejadores_errores(app: FastAPI) -> None:
    app.add_exception_handler(ApplicationError, manejar_error_aplicacion)
    app.add_exception_handler(HTTPException, manejar_http_exception)
    app.add_exception_handler(RequestValidationError, manejar_error_validacion)
