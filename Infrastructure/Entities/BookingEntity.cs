using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class BookingEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "char(20)")]
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

    public StatusEntity Status { get; set; } = null!;
    public ClientEntity Client { get; set; } = null!;
    public ParticipantsEntity Participants { get; set; } = null!;
    public TimeEntity Time { get; set; } = null!;
    public LocationEntity Location { get; set; } = null!;

}
