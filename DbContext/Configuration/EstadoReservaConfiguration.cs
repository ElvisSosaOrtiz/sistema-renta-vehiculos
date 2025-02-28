namespace DbContext.Configuration
{
    using Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Shared.Enums;

    public class EstadoReservaConfiguration : IEntityTypeConfiguration<EstadoReservaEntity>
    {
        public void Configure(EntityTypeBuilder<EstadoReservaEntity> builder)
        {
            builder.HasData(
                new EstadoReservaEntity { Id = 1, Nombre = nameof(EstadosReserva.Confirmada) },
                new EstadoReservaEntity { Id = 2, Nombre = nameof(EstadosReserva.Cancelada) },
                new EstadoReservaEntity { Id = 3, Nombre = nameof(EstadosReserva.Finalizada) }
            );
        }
    }
}
