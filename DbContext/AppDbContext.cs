namespace DbContext
{
    using DbContext.Configuration;
    using Entities;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext(DbContextOptions options) : IdentityDbContext<UsuarioEntity>(options)
    {
        public DbSet<VehiculoEntity> Vehiculos { get; set; }
        public DbSet<EstadoVehiculoEntity> EstadosVehiculo { get; set; }
        public DbSet<UsuarioEntity> Usuarios { get; set; }
        public DbSet<ReservaEntity> Reservas { get; set; }
        public DbSet<EstadoReservaEntity> EstadosReserva { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new EstadoVehiculoConfiguration());
            modelBuilder.ApplyConfiguration(new EstadoReservaConfiguration());
            modelBuilder.ApplyConfiguration(new RolesConfiguration());

            modelBuilder.Entity<UsuarioEntity>(entity =>
            {
                entity.HasIndex(e => e.Cedula).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });
        }
    }
}
