using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAPI.Context;
using MyAPI.DTO;
using MyAPI.Models;

namespace MyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(AppDbContext context, ILogger<CategoriasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAll()
        {
            var categorias = await _context.Categorias.ToListAsync();

            if (categorias is null)
            {
                return NotFound();
            }

            return Ok(categorias);
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            try
            {
                return _context.Categorias.Include(p => p.Produtos).ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao buscar as informações!.");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                return _context.Categorias.AsNoTracking().ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao buscar as informações!.");
            }

        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                var categoria = _context.Categorias.FirstOrDefault(p => p.Id == id);

                if (categoria == null)
                {
                    _logger.LogWarning($"Categoria com id= {id} não encontrada...");
                    return NotFound($"Categoria com id= {id} não encontrada...");
                }
                return Ok(categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                           "Ocorreu um problema ao buscar as informações!.");
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                if (categoriaDto is null)
                {
                    _logger.LogWarning($"Dados inválidos...");
                    return BadRequest("Dados inválidos");
                }

                _context.Add(categoriaDto);
                _context.SaveChanges();

                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoriaDto.Id }, categoriaDto);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                          "Ocorreu um problema ao buscar as informações!.");
            }
        }

        // Esse método é para demonstrar como era utilizado antes da verção 2, na verção desse projeto o FromBody e a validação do ModelState já e feito de forma automática
        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] CategoriaDTO categoriaDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    categoriaDto.Id = id;
                    _context.Update(categoriaDto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound("Erro ao atualizar a categoria.");
                }
                return Ok(categoriaDto);
            }
            return BadRequest();
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, CategoriaDTO categoriaDto)
        {
            try
            {
                if (id != categoriaDto.Id)
                {
                    return BadRequest("Dados inválidos");
                }
                _context.Entry(categoriaDto).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok(categoriaDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                       "Ocorreu um problema ao buscar as informações!.");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var categoria = _context.Categorias.FirstOrDefault(p => p.Id == id);

                if (categoria == null)
                {
                    _logger.LogWarning($"Categoria com id={id} não encontrada...");
                    return NotFound($"Categoria com id={id} não encontrada...");
                }

                _context.Categorias.Remove(categoria);
                _context.SaveChanges();
                return Ok(categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                               "Ocorreu um problema ao buscar as informações!.");
            }
        }
    }

}