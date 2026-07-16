class ApplicationError(Exception):
    """Error esperado de un caso de uso."""


class EmailYaRegistradoError(ApplicationError):
    pass


class CredencialesInvalidasError(ApplicationError):
    pass


class TransaccionNoEncontradaError(ApplicationError):
    pass
