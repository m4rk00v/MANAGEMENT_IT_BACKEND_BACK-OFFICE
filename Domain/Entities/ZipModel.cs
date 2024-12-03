using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class ZipModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public int ModelId { get; set; }
    
    [ForeignKey("ModelId")]
    public virtual Model Model { get; set; }
    
}