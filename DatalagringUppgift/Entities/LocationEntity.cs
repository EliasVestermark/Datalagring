using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatalagringUppgift.Entities;

public class LocationEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string Address { get; set; } = null!;

    [Required]
    [Column(TypeName = "char(6)")]
    public string PostalCode { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string City { get; set; } = null!;

    public virtual ICollection<BookingEntity> BookingEntities { get; set; } = new HashSet<BookingEntity>();
}
