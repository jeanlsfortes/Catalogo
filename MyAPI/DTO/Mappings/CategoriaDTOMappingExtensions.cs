using MyAPI.DTO;
using MyAPI.Models;

namespace MyAPI.DTOs.Mappings;

/// <summary>
/// Métodos para mapear e converter da classe para o DTO.
/// Para atribuir o método na classe:
///    como funciona - no método de exteção a classe que vai receber o método deve ser definida com o modificador this. 
///    exemplo - public static CategoriaDTO? ToCategoriaDTO(this Categoria categoria)
/// </summary>
public static class CategoriaDTOMappingExtensions
{
    // mapeamento entre categoria e CategoriaDTO
    public static CategoriaDTO? ToCategoriaDTO(this Categoria categoria)
    {
        if (categoria is null)
            return null;

        return new CategoriaDTO
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
            ImagemUrl = categoria.ImagemUrl
        };
    }

    // mapeamento entre CategoriaDTO e Categoria
    public static Categoria? ToCategoria(this CategoriaDTO categoriaDto)
    {
        if (categoriaDto is null) return null;

        return new Categoria
        {
            Id = categoriaDto.Id,
            Nome = categoriaDto.Nome,
            ImagemUrl = categoriaDto.ImagemUrl
        };
    }

    // mapeamento entre lista de Categoria e uma lista de CategoriaDTO
    public static IEnumerable<CategoriaDTO> ToCategoriaDTOList(this IEnumerable<Categoria> categorias)
    {
        if (categorias is null || !categorias.Any())
        {
            return new List<CategoriaDTO>();
        }

        return categorias.Select(categoria => new CategoriaDTO
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
            ImagemUrl = categoria.ImagemUrl
        }).ToList();
    }
}
