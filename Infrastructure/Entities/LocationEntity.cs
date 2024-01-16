using System.ComponentModel.DataAnnotations;

namespace DatalagringUppgift.Entities;

public class LocationEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Address { get; set; } = null!;

    [Required]
    [StringLength(6)]
    public string PostalCode { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string City { get; set; } = null!;
}
