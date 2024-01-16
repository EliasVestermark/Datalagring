using System.ComponentModel.DataAnnotations;

namespace DatalagringUppgift.Entities;

public class StatusEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string StatusText { get; set; } = null!;
}
