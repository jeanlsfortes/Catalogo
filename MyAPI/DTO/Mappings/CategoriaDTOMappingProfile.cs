using AutoMapper;
using MyAPI.Models;

namespace MyAPI.DTO.Mappings
{
    public class CategoriaDTOMappingProfile : Profile
    {
        public CategoriaDTOMappingProfile() {

            // Mapeamento bidirecional entre Categoria e CategoriaDTO
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();

        }
    }
}
