namespace DbContext
{
    using DbContext.Configuration;
    using Entities;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<VehiculoEntity> Vehiculos { get; set; }
        public DbSet<EstadoVehiculoEntity> EstadosVehiculo { get; set; }
        public DbSet<UsuarioEntity> Usuarios { get; set; }
        public DbSet<ReservaEntity> Reservas { get; set; }
        public DbSet<EstadoReservaEntity> EstadosReserva { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EstadoVehiculoConfiguration());
            modelBuilder.ApplyConfiguration(new EstadoReservaConfiguration());
        }
    }
}
