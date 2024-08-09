using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MyAPI.Models
{
    public class Categoria : EntityBase
    {
        public Categoria() { 
            Produtos = new Collection<Produto>();
        }
        [Required]
        [StringLength(300)]
        public string? ImagemUrl { get; set; }

        public ICollection<Produto>? Produtos { get; set; }
    }
}
