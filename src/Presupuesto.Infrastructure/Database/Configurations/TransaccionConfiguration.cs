using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Presupuesto.Domain.Transacciones;

namespace Presupuesto.Infrastructure.Database.Configurations;

public sealed class TransaccionConfiguration : IEntityTypeConfiguration<Transaccion>
{
    public void Configure(EntityTypeBuilder<Transaccion> builder)
    {
        builder.ToTable("transacciones");
        builder.HasKey(transaccion => transaccion.Id);

        builder.Property(transaccion => transaccion.UsuarioId)
            .HasColumnName("usuario_id")
            .IsRequired();

        builder.OwnsOne(transaccion => transaccion.Monto, monto =>
        {
            monto.Property(m => m.Monto)
                .HasColumnName("monto")
                .HasPrecision(18, 2)
                .IsRequired();
        });

        builder.OwnsOne(transaccion => transaccion.Descripcion, descripcion =>
        {
            descripcion.Property(d => d.Value)
                .HasColumnName("descripcion")
                .HasMaxLength(255)
                .IsRequired();
        });

        builder.OwnsOne(transaccion => transaccion.Categoria, categoria =>
        {
            categoria.Property(c => c.Value)
                .HasColumnName("categoria")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.Property(transaccion => transaccion.Fecha)
            .HasColumnName("fecha")
            .IsRequired();

        builder.Property(transaccion => transaccion.Tipo)
            .HasColumnName("tipo")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(transaccion => transaccion.CreadoEnUtc)
            .HasColumnName("creado_en_utc")
            .IsRequired();

        builder.HasIndex(transaccion => new { transaccion.UsuarioId, transaccion.Fecha });
        builder.Ignore(transaccion => transaccion.DomainEvents);
    }
}
