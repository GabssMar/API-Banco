using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoDsin.Migrations
{
    /// <inheritdoc />
    public partial class RemoverCamposProprietario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cnh",
                table: "DadosProprietarios");

            migrationBuilder.DropColumn(
                name: "Cpf",
                table: "DadosProprietarios");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "DadosProprietarios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cnh",
                table: "DadosProprietarios",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                table: "DadosProprietarios",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "DadosProprietarios",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
