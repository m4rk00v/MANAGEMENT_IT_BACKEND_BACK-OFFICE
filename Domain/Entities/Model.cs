using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Model
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; }

    public decimal Size { get; set; }

    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; }

    public int ZipModelId { get; set; } // Agregar la clave foránea explícita

    [ForeignKey("ZipModelId")]
    public virtual ZipModel ZipModel { get; set; }

    // Método para convertir un DTO a una entidad
    public static Model FromDto(ModelDto modelDto)
    {
        return new Model
        {
            Id = modelDto.Id,
            Name = modelDto.Name,
            Size = modelDto.Size,
            CreatedAt = DateTime.Now // Puedes ajustar según tus necesidades
        };
    }
}