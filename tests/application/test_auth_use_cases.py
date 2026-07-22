import pytest

from app.application.errors import CredencialesInvalidasError, EmailYaRegistradoError
from app.application.schemas import LoginRequest, UsuarioCreate
from app.application.use_cases.login_usuario import login_usuario
from app.application.use_cases.registrar_usuario import registrar_usuario

from tests.application.fakes import PasswordHasherFake, TokenServiceFake, UsuarioRepositoryFake


def test_registrar_usuario_crea_usuario_con_password_hasheado():
    usuarios = UsuarioRepositoryFake()
    password_hasher = PasswordHasherFake()
    payload = UsuarioCreate(
        nombre="Ana",
        email="ana@example.com",
        password="secreto123",
    )

    usuario = registrar_usuario(payload, usuarios, password_hasher)

    assert usuario.id == 1
    assert usuario.email == "ana@example.com"
    assert usuario.password_hash == "hashed:secreto123"


def test_registrar_usuario_rechaza_email_duplicado():
    usuarios = UsuarioRepositoryFake()
    password_hasher = PasswordHasherFake()
    payload = UsuarioCreate(
        nombre="Ana",
        email="ana@example.com",
        password="secreto123",
    )
    registrar_usuario(payload, usuarios, password_hasher)

    with pytest.raises(EmailYaRegistradoError):
        registrar_usuario(payload, usuarios, password_hasher)


def test_login_usuario_devuelve_token_si_credenciales_son_validas():
    usuarios = UsuarioRepositoryFake()
    password_hasher = PasswordHasherFake()
    token_service = TokenServiceFake()
    registrar_usuario(
        UsuarioCreate(
            nombre="Ana",
            email="ana@example.com",
            password="secreto123",
        ),
        usuarios,
        password_hasher,
    )

    token = login_usuario(
        LoginRequest(email="ana@example.com", password="secreto123"),
        usuarios,
        password_hasher,
        token_service,
    )

    assert token.access_token == "token:1"
    assert token.token_type == "bearer"


def test_login_usuario_rechaza_password_incorrecto():
    usuarios = UsuarioRepositoryFake()
    password_hasher = PasswordHasherFake()
    token_service = TokenServiceFake()
    registrar_usuario(
        UsuarioCreate(
            nombre="Ana",
            email="ana@example.com",
            password="secreto123",
        ),
        usuarios,
        password_hasher,
    )

    with pytest.raises(CredencialesInvalidasError):
        login_usuario(
            LoginRequest(email="ana@example.com", password="incorrecto"),
            usuarios,
            password_hasher,
            token_service,
        )
