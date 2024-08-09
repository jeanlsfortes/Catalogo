using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAPI.Models
{
    public class Produto : EntityBase
    {
        [Required]
        [StringLength(100)]
        public string? Descricao { get; set; }
        [Column(TypeName = "decimal(18, 2")]
        public decimal Preco { get; set; }
        public string? ImagemUrl { get; set; }
        public float Estoque { get; set; }
        public DateTime DactaCadastro { get; set; }
        [Key]
        public int? CategoriaId { get; set; }
        public Categoria? Categoria {  get; set; } 
    }
}
