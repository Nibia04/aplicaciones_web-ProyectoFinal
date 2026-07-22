using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Presupuesto.Domain.Usuarios;

namespace Presupuesto.Infrastructure.Database.Configurations;

public sealed class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuarios");
        builder.HasKey(usuario => usuario.Id);

        builder.OwnsOne(usuario => usuario.Nombre, nombre =>
        {
            nombre.Property(n => n.Value)
                .HasColumnName("nombre")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.OwnsOne(usuario => usuario.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired();
            email.HasIndex(e => e.Value).IsUnique();
        });

        builder.Property(usuario => usuario.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(usuario => usuario.CreadoEnUtc)
            .HasColumnName("creado_en_utc")
            .IsRequired();

        builder.Ignore(usuario => usuario.DomainEvents);
        builder.Metadata.FindNavigation(nameof(Usuario.Transacciones))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
