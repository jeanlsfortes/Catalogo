using MyAPI.Context;
using MyAPI.Models;
using MyAPI.Pagination;
using X.PagedList;
using X.PagedList.Extensions;

namespace MyAPI.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context)
    { }

    public async Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters 
                                                                   categoriasParams)
    {
        var categorias = await GetAllAsync();

        var categoriasOrdenadas = categorias.OrderBy(p => p.Id).ToList();

        var resultado = categoriasOrdenadas.ToPagedList(categoriasParams.PageNumber, categoriasParams.PageSize);

        return resultado;
    }

    public async Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(
        CategoriasFiltroNome categoriasParams)
    {
        var categorias = await GetAllAsync();

        if (!string.IsNullOrEmpty(categoriasParams.Nome))
        {
            categorias = categorias.Where(c => c.Nome.Contains(categoriasParams.Nome));
        }

        var categoriasFiltradas = categorias.ToPagedList(
                                             categoriasParams.PageNumber,
                                             categoriasParams.PageSize);

        return categoriasFiltradas;
    }
}
