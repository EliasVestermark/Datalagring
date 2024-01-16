using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatalagringUppgift.Entities;

public class BookingEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "char(10)")]
    public string Date { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(StatusEntity))]
    public int StatusId { get; set; }

    [Required]
    [ForeignKey(nameof(ClientEntity))]
    public int ClientId { get; set; }

    [Required]
    [ForeignKey(nameof(ParticipantsEntity))]
    public int ParticipantsId { get; set; }

    [Required]
    [ForeignKey(nameof(TimeEntity))]
    public int TimeId { get; set; }

    [Required]
    [ForeignKey(nameof(LocationEntity))]
    public int LocationId { get; set; }

    public virtual StatusEntity Status { get; set; } = null!;
    public virtual ClientEntity Client { get; set; } = null!;
    public virtual ParticipantsEntity Participants { get; set; } = null!;
    public virtual TimeEntity Time { get; set; } = null!;
    public virtual LocationEntity Location { get; set; } = null!;

}
