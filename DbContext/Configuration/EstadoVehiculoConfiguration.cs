namespace DbContext.Configuration
{
    using Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Shared.Enums;

    public class EstadoVehiculoConfiguration : IEntityTypeConfiguration<EstadoVehiculoEntity>
    {
        public void Configure(EntityTypeBuilder<EstadoVehiculoEntity> builder)
        {
            builder.HasData(
                new EstadoVehiculoEntity { Id = 1, Nombre = nameof(EstadosVehiculo.Disponible) },
                new EstadoVehiculoEntity { Id = 2, Nombre = nameof(EstadosVehiculo.Rentado) },
                new EstadoVehiculoEntity { Id = 3, Nombre = nameof(EstadosVehiculo.Mantenimiento) }
            );
        }
    }
}
