using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaFornecedores : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO Fornecedores(Nome, Cnpj, Telefone, Endereco) " +
                   "VALUES('Fornecedor A', '12345678000101', '11987654321', 'Rua A, 123 - São Paulo, SP')");

            mb.Sql("INSERT INTO Fornecedores(Nome, Cnpj, Telefone, Endereco) " +
                   "VALUES('Fornecedor B', '98765432000102', '11987654322', 'Rua B, 456 - Rio de Janeiro, RJ')");

            mb.Sql("INSERT INTO Fornecedores(Nome, Cnpj, Telefone, Endereco) " +
                   "VALUES('Fornecedor C', '12312345000103', '11987654323', 'Avenida C, 789 - Belo Horizonte, MG')");

            mb.Sql("INSERT INTO Fornecedores(Nome, Cnpj, Telefone, Endereco) " +
                   "VALUES('Fornecedor D', '98798765000104', '11987654324', 'Rua D, 321 - Curitiba, PR')");

            mb.Sql("INSERT INTO Fornecedores(Nome, Cnpj, Telefone, Endereco) " +
                   "VALUES('Fornecedor E', '45645678000105', '11987654325', 'Avenida E, 654 - Salvador, BA')");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("DELETE FROM Fornecedores");
        }
    }
}
