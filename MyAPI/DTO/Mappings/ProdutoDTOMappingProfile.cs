using AutoMapper;
using MyAPI.DTO;
using MyAPI.Models;

namespace MyAPI.DTOs.Mappings;

public class ProdutoDTOMappingProfile : Profile
{ 
    public ProdutoDTOMappingProfile()
    {
        // Mapeamento bidirecional entre Produto e ProdutoDTO
        CreateMap<Produto, ProdutoDTO>().ReverseMap();

        // Mapeamento bidirecional entre Categoria e CategoriaDTO
        CreateMap<Categoria, CategoriaDTO>().ReverseMap();

        // Mapeamento bidirecional entre Produto e ProdutoDTOUpdateRequest
        CreateMap<Produto, ProdutoDTOUpdateRequest>().ReverseMap();

        // Mapeamento bidirecional entre Produto e ProdutoDTOUpdateResponse
        CreateMap<Produto, ProdutoDTOUpdateResponse>().ReverseMap();
    }
}
