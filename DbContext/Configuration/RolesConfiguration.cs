namespace DbContext.Configuration
{
    using Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Shared.Enums;

    public class RolesConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "e81bcd53-c920-4d4e-9b5e-d60bda5d91d6",
                    Name = nameof(UserRoles.Administrador),
                    NormalizedName = nameof(UserRoles.Administrador).ToUpper()
                },
                new IdentityRole
                {
                    Id = "82cde227-6050-4686-a011-eeeefa43e4fa",
                    Name = nameof(UserRoles.Cliente),
                    NormalizedName = nameof(UserRoles.Cliente).ToUpper()
                }
            );
        }
    }
}
