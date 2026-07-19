from app.application.errors import EmailYaRegistradoError
from app.application.ports import PasswordHasher, UsuarioRepository
from app.application.schemas import UsuarioCreate


def registrar_usuario(
    payload: UsuarioCreate,
    usuarios: UsuarioRepository,
    password_hasher: PasswordHasher,
):
    if usuarios.obtener_por_email(payload.email):
        raise EmailYaRegistradoError("El email ya esta registrado")

    return usuarios.crear(
        nombre=payload.nombre,
        email=payload.email,
        password_hash=password_hasher.hash(payload.password),
    )
