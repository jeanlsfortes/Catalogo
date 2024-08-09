using System.ComponentModel.DataAnnotations;

namespace MyAPI.Models
{
    public class EntityBase
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Nome { get; set; } = string.Empty;

        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
