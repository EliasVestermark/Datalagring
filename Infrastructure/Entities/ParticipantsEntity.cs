using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class ParticipantsEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "varchar(7)")]
    public string Amount { get; set; } = null!;

    public virtual ICollection<BookingEntity> Bookings { get; set; } = new HashSet<BookingEntity>();
}
