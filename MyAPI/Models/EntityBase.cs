using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAPI.Models
{
    public class EntityBase
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public EntityBase()
        {
            CreateAt = DateTime.Now;  // Inicializa CreateAt com a data e hora atual
        }
    }
}
