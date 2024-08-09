using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyAPI.Migrations
{
    /// <inheritdoc />
    public partial class PopulatesDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insere categorias primeiro
            migrationBuilder.Sql("INSERT INTO Categorias(CreateAt, Nome, ImagemUrl) VALUES (GETDATE(), 'Bebidas', 'bebidas.jpg')");
            migrationBuilder.Sql("INSERT INTO Categorias(CreateAt, Nome, ImagemUrl) VALUES (GETDATE(), 'Lanches', 'lanches.jpg')");
            migrationBuilder.Sql("INSERT INTO Categorias(CreateAt, Nome, ImagemUrl) VALUES (GETDATE(), 'Sobremesas', 'sobremesas.jpg')");

            // Insere produtos depois
            migrationBuilder.Sql("INSERT INTO Produtos(CreateAt, Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) VALUES (GETDATE(), 'Coca-Cola Diet', 'Refrigerante de Cola 350 ml', 5.45, 'cocacola.jpg', 50, GETDATE(), (SELECT Id FROM Categorias WHERE Nome = 'Bebidas'))");
            migrationBuilder.Sql("INSERT INTO Produtos(CreateAt, Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) VALUES (GETDATE(), 'Lanche de Atum', 'Lanche de Atum com maionese', 8.50, 'atum.jpg', 10, GETDATE(), (SELECT Id FROM Categorias WHERE Nome = 'Lanches'))");
            migrationBuilder.Sql("INSERT INTO Produtos(CreateAt, Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) VALUES (GETDATE(), 'Pudim 100 g', 'Pudim de leite condensado 100g', 6.75, 'pudim.jpg', 20, GETDATE(), (SELECT Id FROM Categorias WHERE Nome = 'Sobremesas'))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Produtos");
            migrationBuilder.Sql("DELETE FROM Categorias");
        }

    }
}
