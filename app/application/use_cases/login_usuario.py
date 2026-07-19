from app.application.errors import CredencialesInvalidasError
from app.application.ports import PasswordHasher, TokenService, UsuarioRepository
from app.application.schemas import LoginRequest, TokenOut


def login_usuario(
    payload: LoginRequest,
    usuarios: UsuarioRepository,
    password_hasher: PasswordHasher,
    token_service: TokenService,
) -> TokenOut:
    usuario = usuarios.obtener_por_email(payload.email)
    if not usuario or not password_hasher.verificar(payload.password, usuario.password_hash):
        raise CredencialesInvalidasError("Credenciales invalidas")

    token = token_service.crear_access_token(subject=str(usuario.id))
    return TokenOut(access_token=token)
