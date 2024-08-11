using System.ComponentModel.DataAnnotations;

namespace MyAPI.DTO
{
    public class ProdutoDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(80)]
        public string? Nome { get; set; }

        [Required]
        [StringLength(300)]
        public string? Descricao { get; set; }
        [Required]

        public decimal Preco { get; set; }

        [Required]
        [StringLength(300)]
        public string? ImagemUrl { get; set; }

        public float Estoque { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true,DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime DataCadastro { get; set; }

        public int CategoriaId { get; set; }
    }
}
