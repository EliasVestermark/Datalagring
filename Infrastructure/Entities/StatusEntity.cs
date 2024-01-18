using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class StatusEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(20)")]
    public string StatusText { get; set; } = null!;

    public virtual ICollection<BookingEntity> Bookings { get; set; } = new HashSet<BookingEntity>();
}
