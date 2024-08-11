using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyAPI.Models
{
    public class Produto : EntityBase
    {
        [Required]
        [StringLength(100)]
        public string? Descricao { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Preco { get; set; }
        public string? ImagemUrl { get; set; }
        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }
        public int? CategoriaId { get; set; }
        [JsonIgnore]
        public Categoria? Categoria {  get; set; } 
    }
}
