using System.ComponentModel.DataAnnotations;

namespace DatalagringUppgift.Entities;

public class ClientEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(30)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(30)]
    public string LastName { get; set; } = null!;

    [Required]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Email { get; set; } = null!;
}
