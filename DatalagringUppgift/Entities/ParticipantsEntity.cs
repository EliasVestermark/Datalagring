using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatalagringUppgift.Entities;

public class ParticipantsEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "varchar(7)")]
    public string Amount { get; set; } = null!;

    public virtual ICollection<BookingEntity> BookingEntities { get; set; } = new HashSet<BookingEntity>();
}
