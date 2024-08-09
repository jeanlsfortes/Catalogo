using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAPI.Context;
using MyAPI.DTO;
using MyAPI.Models;

namespace MyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CategoriasController> _logger;
        public ProdutosController(AppDbContext context, ILogger<CategoriasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context.Produtos.ToList();
            if (produtos is null)
            {
                return NotFound();
            }
            return produtos;
        }

        [HttpGet("GetAsync")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
        {
            var produtos = await _context.Produtos.ToListAsync();
            if (produtos is null)
            {
                _logger.LogWarning("Não foram encontrados Produtos no banco...");
                return NotFound();
            }
            return produtos;
        }


        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == id);
            if (produto is null)
            {
                _logger.LogWarning("Produto não encontrado...");
                return NotFound("Produto não encontrado...");
            }
            return produto;
        }

        [HttpGet("Async/{id:int}", Name = "ObterProdutoAsync")]
        public async Task<ActionResult<Produto>> GetAsync(int id)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);
            if (produto is null)
            {
                _logger.LogWarning("Produto não encontrado...");
                return NotFound("Produto não encontrado...");
            }
            return produto;
        }

        [HttpPost]
        public ActionResult Post(ProdutoDTO produto)
        {
            if (produto is null)
                return BadRequest();

            _context.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.Id }, produto);
        }

        [HttpPost("PostAsync")]
        public async Task<ActionResult> PostAsync(ProdutoDTO produto)
        {
            if (produto is null)
                return BadRequest();

            _context.Add(produto);
            await _context.SaveChangesAsync();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.Id }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, ProdutoDTO produto)
        {
            if (id != produto.Id)
            {
                return BadRequest();
            }

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpPut("Async/{id:int}")]
        public async Task<ActionResult> PutAsync(int id, ProdutoDTO produto)
        {
            if (id != produto.Id)
            {
                return BadRequest();
            }

            var produtoEntity = await _context.Produtos.FindAsync(id);
            if (produtoEntity is null)
            {
                return NotFound("Produto não localizado...");
            }

            produtoEntity.Nome = produto.Nome;
            produtoEntity.Descricao = produto.Descricao;
            produtoEntity.Preco = produto.Preco;
            produtoEntity.ImagemUrl = produto.ImagemUrl;
            produtoEntity.Estoque = produto.Estoque;
            produtoEntity.DataCadastro = produto.DataCadastro;
            produtoEntity.CategoriaId = produto.CategoriaId;

            _context.Entry(produtoEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(produtoEntity);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == id);

            if (produto is null)
            {
                return NotFound("Produto não localizado...");
            }
            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("Async/{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto is null)
            {
                return NotFound("Produto não localizado...");
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return Ok(produto);
        }
    }
}
