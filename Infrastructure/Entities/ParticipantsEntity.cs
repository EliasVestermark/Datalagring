using System.ComponentModel.DataAnnotations;

namespace DatalagringUppgift.Entities;

public class ParticipantsEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string Amount { get; set; } = null!;
}
