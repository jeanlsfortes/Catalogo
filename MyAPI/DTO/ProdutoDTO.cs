using MyAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyAPI.DTO
{
    public class ProdutoDTO
    {
        public int Id { get; set; }
        public string? Descricao { get; set; }

        public decimal? Preco { get; set; }
        public string? ImagemUrl { get; set; }
        public float? Estoque { get; set; }
        public DateTime? DataCadastro { get; set; }
        public int? CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
    }
}
