using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Adapters;
using Microsoft.AspNetCore.Mvc;
using MyAPI.DTO;
using MyAPI.DTOs;
using MyAPI.Models;
using MyAPI.Pagination;
using MyAPI.Repositories;
using Newtonsoft.Json;
using X.PagedList;

namespace MyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(IUnitOfWork uof, IMapper mapper, ILogger<ProdutosController> logger)
        {
            _uof = uof;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("produtos/{id}")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosCategoria(int id)
        {

            var produtos = await _uof.ProdutoRepository.GetProdutosPorCategoriaAsync(id);

            if (produtos is null)
            {
                _logger.LogWarning($"Não foi possível obter o produto {id}");
                return NotFound("Não existem Produtos...");
            }

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDto);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery]
                                   ProdutosParameters produtosParameters)
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosAsync(produtosParameters);

            return ObterProdutos(produtos);
        }

        [HttpGet("filter/preco/pagination")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosFilterPreco([FromQuery] ProdutosFiltroPreco
                                                                                        produtosFilterParameters)
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosFiltroPrecoAsync(produtosFilterParameters);
            return ObterProdutos(produtos);
        }
        private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos(IPagedList<Produto> produtos)
        {
            try
            {
                var metadata = new
                {
                    produtos.Count,
                    produtos.PageSize,
                    produtos.PageCount,
                    produtos.TotalItemCount,
                    produtos.HasNextPage,
                    produtos.HasPreviousPage
                };

                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
                return Ok(produtosDto);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($" ObterProdutos - Não foi possível obter os produtos : {ex.Message}");
                return BadRequest("Não foi possível obter os produtos");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get()
        {
            var produtos = await _uof.ProdutoRepository.GetAllAsync();
            if (produtos is null)
            {
                _logger.LogWarning("Não existem Produtos...");
                return NotFound("Não existem Produtos...");
            }

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDto);
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {
            var produto = await _uof.ProdutoRepository.GetAsync(c => c.Id == id);
            if (produto is null)
            {
                _logger.LogWarning("Get - Produto não encontrado...");
                return NotFound("Produto não encontrado...");
            }
            var produtoDto = _mapper.Map<ProdutoDTO>(produto);
            return Ok(produtoDto);
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoDTO>> Post(ProdutoDTO produtoDto)
        {
            if (produtoDto is null)
            {
                _logger.LogWarning("Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            var produto = _mapper.Map<Produto>(produtoDto);

            var novoProduto = _uof.ProdutoRepository.Create(produto);
            await _uof.Commit();

            var novoProdutoDto = _mapper.Map<ProdutoDTO>(novoProduto);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = novoProdutoDto.Id }, novoProdutoDto);
        }

        [HttpPatch("{id}/UpdatePartial")]
        public async Task<ActionResult<ProdutoDTOUpdateResponse>> Patch(int id,
            JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDto)
        {
            if (patchProdutoDto == null || id <= 0)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            var produto = await _uof.ProdutoRepository.GetAsync(c => c.Id == id);

            if (produto == null)
                return NotFound();

            var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

            patchProdutoDto.ApplyTo(produtoUpdateRequest, (IObjectAdapter)ModelState);

            if (!ModelState.IsValid || !TryValidateModel(produtoUpdateRequest))
            {
                _logger.LogWarning(ModelState.ToString());
                return BadRequest(ModelState);
            }

            _mapper.Map(produtoUpdateRequest, produto);

            _uof.ProdutoRepository.Update(produto);
            await _uof.Commit();

            return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> Put(int id, ProdutoDTO produtoDto)
        {
            if (id != produtoDto.Id)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            var produto = _mapper.Map<Produto>(produtoDto);

            var produtoAtualizado = _uof.ProdutoRepository.Update(produto);
            await _uof.Commit();

            var produtoAtualizadoDto = _mapper.Map<ProdutoDTO>(produtoAtualizado);

            return Ok(produtoAtualizadoDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {
            var produto = await _uof.ProdutoRepository.GetAsync(p => p.Id == id);
            if (produto is null)
            {
                _logger.LogWarning("Produto não encontrado");
                return NotFound("Produto não encontrado.");
            }

            var produtoDeletado = _uof.ProdutoRepository.Delete(produto);
            await _uof.Commit();

            var produtoDeletadoDto = _mapper.Map<ProdutoDTO>(produtoDeletado);

            return Ok(produtoDeletadoDto);
        }
    }
}
