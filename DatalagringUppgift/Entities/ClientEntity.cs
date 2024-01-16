using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatalagringUppgift.Entities;

public class ClientEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(30)")]
    public string FirstName { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(30)")]
    public string LastName { get; set; } = null!;

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [Column(TypeName = "varchar(100)")]
    public string Email { get; set; } = null!;

    public virtual ICollection<BookingEntity> BookingEntities { get; set; } = new HashSet<BookingEntity>();
}
