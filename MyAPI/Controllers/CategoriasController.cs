using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAPI.Context;
using MyAPI.DTO;
using MyAPI.DTOs.Mappings;
using MyAPI.Models;
using MyAPI.Pagination;
using MyAPI.Repositories;
using Newtonsoft.Json;
using X.PagedList;

namespace MyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriasController : Controller
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(IUnitOfWork uof, IMapper mapper, ILogger<CategoriasController> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _uof = uof;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            var categorias = await _uof.CategoriaRepository.GetAllAsync();

            if (categorias is null)
            {
                _logger.LogWarning("Não existem categorias...");
                return NotFound("Não existem categorias...");
            }

            var categoriasDto = categorias.ToCategoriaDTOList();

            return Ok(categoriasDto);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery]
                               CategoriasParameters categoriasParameters)
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasAsync(categoriasParameters);

            return ObterCategorias(categorias);
        }

        [HttpGet("filter/nome/pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasFiltradas([FromQuery] CategoriasFiltroNome categoriasFiltro)
        {
            var categoriasFiltradas = await _uof.CategoriaRepository.GetCategoriasFiltroNomeAsync(categoriasFiltro);

            if (categoriasFiltradas == null || !categoriasFiltradas.Any())
            {
                _logger.LogWarning("Nenhuma categoria encontrada com os filtros fornecidos.");
                return NotFound("Nenhuma categoria encontrada com os filtros fornecidos.");
            }

            return Ok(_mapper.Map<IEnumerable<CategoriaDTO>>(categoriasFiltradas));
        }

        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(IPagedList<Categoria> categorias)
        {
            try
            {
                var metadata = new
                {
                    categorias.Count,
                    categorias.PageSize,
                    categorias.PageCount,
                    categorias.TotalItemCount,
                    categorias.HasNextPage,
                    categorias.HasPreviousPage
                };

                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                var categoriasDto = categorias.ToCategoriaDTOList();

                return Ok(categoriasDto);

            }
            catch (Exception ex) 
            {
                _logger.LogWarning($"Não foi possível obter as categorias : {ex.Message}");
                return NotFound("Não foi possível obter as categorias");
            }
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetAsync(c => c.Id == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrada...");
                return NotFound($"Categoria com id= {id} não encontrada...");
            }

            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoriaDto);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                _logger.LogWarning("Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            var categoria = _mapper.Map<Categoria>(categoriaDto);

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            await _uof.Commit();

            var novaCategoriaDto = _mapper.Map<CategoriaDTO>(categoriaCriada);

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = novaCategoriaDto.Id },
                novaCategoriaDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.Id)
            {
                _logger.LogWarning("Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            var categoria = _mapper.Map<Categoria>(categoriaDto);

            var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
            await _uof.Commit();

            var categoriaAtualizadaDto = _mapper.Map<CategoriaDTO>(categoriaAtualizada);

            return Ok(categoriaAtualizadaDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetAsync(c => c.Id == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id={id} não encontrada...");
                return NotFound($"Categoria com id={id} não encontrada...");
            }

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            await _uof.Commit();

            var categoriaExcluidaDto = _mapper.Map<CategoriaDTO>(categoriaExcluida);

            return Ok(categoriaExcluidaDto);
        }
    }
}