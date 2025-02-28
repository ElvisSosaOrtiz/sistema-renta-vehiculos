namespace Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Usuario")]
    public class UsuarioEntity
    {
        [Key]
        [Required]
        public string Cedula { get; set; } = null!;

        [Required]
        public string Nombre { get; set; } = null!;

        [Required]
        public string Apellido { get; set; } = null!;

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; } = null!;

        [Required]
        public string Direccion { get; set; } = null!;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; } = null!;
    }
}
