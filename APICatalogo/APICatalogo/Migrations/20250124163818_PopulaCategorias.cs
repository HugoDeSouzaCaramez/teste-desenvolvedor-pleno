using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaCategorias : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO Categorias(Nome, Descricao) " +
                    "VALUES('Bebidas', 'Bebidas variadas, incluindo refrigerantes, sucos e bebidas alcoólicas.')");

            mb.Sql("INSERT INTO Categorias(Nome, Descricao) " +
                   "VALUES('Lanches', 'Snacks e alimentos prontos, como salgadinhos, biscoitos e sanduíches.')");

            mb.Sql("INSERT INTO Categorias(Nome, Descricao) " +
                   "VALUES('Sobremesas', 'Doces, bolos, sorvetes e outras opções de sobremesa.')");

            mb.Sql("INSERT INTO Categorias(Nome, Descricao) " +
                   "VALUES('Produtos de Limpeza', 'Produtos para a limpeza doméstica, como detergentes, desinfetantes e esponjas.')");

            mb.Sql("INSERT INTO Categorias(Nome, Descricao) " +
                   "VALUES('Eletrônicos', 'Aparelhos eletrônicos, incluindo celulares, notebooks e acessórios.')");

            mb.Sql("INSERT INTO Categorias(Nome, Descricao) " +
                   "VALUES('Higiene Pessoal', 'Itens para cuidados pessoais, como sabonetes, shampoos e creme dental.')");

        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Categorias");
        }
    }
}
