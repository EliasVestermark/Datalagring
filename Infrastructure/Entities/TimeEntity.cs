using System.ComponentModel.DataAnnotations;

namespace DatalagringUppgift.Entities
{
    public class TimeEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(4)]
        public string StartTime { get; set; } = null!;

        [Required]
        [StringLength(4)]
        public string EndTime { get; set;} = null!;
    }
}
