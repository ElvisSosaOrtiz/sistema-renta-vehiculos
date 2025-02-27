namespace Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Administrador")]
    public class AdministradorEntity
    {
        [Key]
        [Required]
        public string Cedula { get; set; } = null!;

        [Required]
        public string Nombre { get; set; } = null!;

        [Required]
        public string Apellido { get; set; } = null!;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; } = null!;
    }
}
