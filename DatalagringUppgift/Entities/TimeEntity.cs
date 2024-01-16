using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatalagringUppgift.Entities
{
    public class TimeEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "char(4)")]
        public string StartTime { get; set; } = null!;

        [Required]
        [Column(TypeName = "char(4)")]
        public string EndTime { get; set;} = null!;

        public virtual ICollection<BookingEntity> BookingEntities { get; set; } = new HashSet<BookingEntity>();
    }
}
