var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();

var presupuestoDb = postgres.AddDatabase("presupuesto");

builder.AddProject<Projects.Presupuesto_Api>("api")
    .WithReference(presupuestoDb)
    .WaitFor(presupuestoDb);

builder.Build().Run();
