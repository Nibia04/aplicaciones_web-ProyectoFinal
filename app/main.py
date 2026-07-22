from fastapi import FastAPI

from app.api.error_handlers import registrar_manejadores_errores
from app.api.routes import auth, presupuesto, transacciones
from app.infrastructure import orm_models
from app.infrastructure.database import Base, engine

Base.metadata.create_all(bind=engine)

app = FastAPI(
    title="Gestor de Presupuesto Diario",
    description="Backend FastAPI para registrar ingresos, gastos y consultar saldo diario.",
    version="0.1.0",
)

registrar_manejadores_errores(app)

app.include_router(auth.router)
app.include_router(transacciones.router)
app.include_router(presupuesto.router)


@app.get("/")
def root():
    return {"mensaje": "API de presupuesto diario funcionando"}


@app.get("/health")
def health_check():
    return {"status": "ok"}
