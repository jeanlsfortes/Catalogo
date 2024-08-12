using Moq;
using MyAPI.Controllers;
using MyAPI.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using MyAPI.DTO;
using MyAPI.Models;
using AutoMapper;
using MyAPI.Pagination;
using X.PagedList;

public class CategoriasControllerTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<CategoriasController>> _mockLogger;
    private readonly Mock<IUnitOfWork> _mockUof;
    private readonly CategoriasController _controller;

    public CategoriasControllerTests()
    {
        _mockLogger = new Mock<ILogger<CategoriasController>>();
        _mockMapper = new Mock<IMapper>();
        _mockUof = new Mock<IUnitOfWork>();

        _controller = new CategoriasController(_mockUof.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Get_ReturnsOkResult_WithCategoriaDTOList()
    {
        // Arrange
        var categoriaList = new List<Categoria> { new Categoria { Id = 12, Nome = "Bebidas" } };
        var categoriaDtoList = new List<CategoriaDTO> { new CategoriaDTO { Id = 13, Nome = "Lanches" } };

        _mockUof.Setup(u => u.CategoriaRepository.GetAllAsync())
                .ReturnsAsync(categoriaList);

        _mockMapper.Setup(m => m.Map<IEnumerable<CategoriaDTO>>(categoriaList))
                   .Returns(categoriaDtoList);

        // Act
        var result = await _controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var resultList = Assert.IsType<List<CategoriaDTO>>(okResult.Value);
        Assert.Equal(categoriaDtoList, resultList);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenNoCategoriasExist()
    {
        // Arrange
        _mockUof.Setup(u => u.CategoriaRepository.GetAllAsync())
                .ReturnsAsync((List<Categoria>)null);

        // Act
        var result = await _controller.Get();

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetCategoriasFiltradas_ReturnsOkResult_WithFilteredCategoriaDTOList()
    {
        // Arrange
        var filtros = new CategoriasFiltroNome { Nome = "Sobremesas" };
        var categoriasFiltradas = new List<Categoria> { new Categoria { Id = 13, Nome = "Lanches" }};
        var categoriaDtoList = new List<CategoriaDTO> { new CategoriaDTO { Id = 14, Nome = "Sobremesas" }};

        var pagedList = new MyAPI.Pagination.PagedList<Categoria>(categoriasFiltradas, 1, 10, categoriasFiltradas.Count) as IPagedList<Categoria>;


        _mockUof.Setup(u => u.CategoriaRepository.GetCategoriasFiltroNomeAsync(filtros))
                .ReturnsAsync(pagedList);

        _mockMapper.Setup(m => m.Map<IEnumerable<CategoriaDTO>>(categoriasFiltradas))
                   .Returns(categoriaDtoList);

        // Act
        var result = await _controller.GetCategoriasFiltradas(filtros);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var resultList = Assert.IsType<List<CategoriaDTO>>(okResult.Value);
        Assert.Equal(categoriaDtoList, resultList);
    }

    [Fact]
    public async Task Get_ReturnsOkResult_WithCategoriaDTO()
    {
        // Arrange
        var id = 12;
        var categoria = new Categoria { Id = 13, Nome = "Lanches" };
        var categoriaDto = new CategoriaDTO { Id = 14, Nome = "Sobremesas" };

        _mockUof.Setup(u => u.CategoriaRepository.GetAsync(c => c.Id == id))
                .ReturnsAsync(categoria);

        _mockMapper.Setup(m => m.Map<CategoriaDTO>(categoria))
                   .Returns(categoriaDto);

        // Act
        var result = await _controller.Get(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var resultDto = Assert.IsType<CategoriaDTO>(okResult.Value);
        Assert.Equal(categoriaDto, resultDto);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenCategoriaDoesNotExist()
    {
        // Arrange
        var id = 12;
        _mockUof.Setup(u => u.CategoriaRepository.GetAsync(c => c.Id == id))
                .ReturnsAsync((Categoria)null);

        // Act
        var result = await _controller.Get(id);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task Post_ReturnsCreatedAtRouteResult_WithCategoriaDTO()
    {
        // Arrange
        var categoriaDto = new CategoriaDTO { Id = 1, Nome = "Categoria Teste" };
        var categoria = new Categoria { Id = 1, Nome = "Categoria Teste" };
        var categoriaCriada = new Categoria { Id = 1, Nome = "Categoria Teste" };

        _mockMapper.Setup(m => m.Map<Categoria>(categoriaDto))
                   .Returns(categoria);

        _mockUof.Setup(u => u.CategoriaRepository.Create(categoria))
                .Returns(categoriaCriada);

        _mockUof.Setup(u => u.Commit())
                .Returns(Task.CompletedTask);

        _mockMapper.Setup(m => m.Map<CategoriaDTO>(categoriaCriada))
                   .Returns(categoriaDto);

        // Act
        var result = await _controller.Post(categoriaDto);

        // Assert
        var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
        var resultDto = Assert.IsType<CategoriaDTO>(createdAtRouteResult.Value);
        Assert.Equal(categoriaDto, resultDto);
    }

    [Fact]
    public async Task Put_ReturnsOkResult_WithUpdatedCategoriaDTO()
    {
        // Arrange
        var id = 12;
        var categoriaDto = new CategoriaDTO { Id = id, Nome = "Categoria Atualizada" };
        var categoria = new Categoria { Id = id, Nome = "Categoria Atualizada" };
        var categoriaAtualizada = new Categoria { Id = id, Nome = "Categoria Atualizada" };
        var categoriaAtualizadaDto = new CategoriaDTO { Id = id, Nome = "Categoria Atualizada" };

        _mockMapper.Setup(m => m.Map<Categoria>(categoriaDto))
                   .Returns(categoria);

        _mockUof.Setup(u => u.CategoriaRepository.Update(categoria))
                .Returns(categoriaAtualizada);

        _mockUof.Setup(u => u.Commit())
                .Returns(Task.CompletedTask);

        _mockMapper.Setup(m => m.Map<CategoriaDTO>(categoriaAtualizada))
                   .Returns(categoriaAtualizadaDto);

        // Act
        var result = await _controller.Put(id, categoriaDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var resultDto = Assert.IsType<CategoriaDTO>(okResult.Value);
        Assert.Equal(categoriaAtualizadaDto, resultDto);
    }

    [Fact]
    public async Task Put_ReturnsBadRequest_WhenIdsDoNotMatch()
    {
        // Arrange
        var id = 12;
        var categoriaDto = new CategoriaDTO { Id = 2, Nome = "Categoria Teste" }; 

        // Act
        var result = await _controller.Put(id, categoriaDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Delete_ReturnsOkResult_WithDeletedCategoriaDTO()
    {
        // Arrange
        var id = 12;
        var categoria = new Categoria { Id = id, Nome = "Categoria Teste" };
        var categoriaDto = new CategoriaDTO { Id = id, Nome = "Categoria Teste" };

        _mockUof.Setup(u => u.CategoriaRepository.GetAsync(c => c.Id == id))
                .ReturnsAsync(categoria);

        _mockUof.Setup(u => u.CategoriaRepository.Delete(categoria))
                .Returns(categoria);

        _mockUof.Setup(u => u.Commit())
                .Returns(Task.CompletedTask);

        _mockMapper.Setup(m => m.Map<CategoriaDTO>(categoria))
                   .Returns(categoriaDto);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var resultDto = Assert.IsType<CategoriaDTO>(okResult.Value);
        Assert.Equal(categoriaDto, resultDto);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenCategoriaDoesNotExist()
    {
        // Arrange
        var id = 122;
        _mockUof.Setup(u => u.CategoriaRepository.GetAsync(c => c.Id == id))
                .ReturnsAsync((Categoria)null);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}
