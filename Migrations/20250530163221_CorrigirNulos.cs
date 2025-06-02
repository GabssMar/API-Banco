using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoDsin.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirNulos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DadosVeiculos_Usuarios_UsuarioId",
                table: "DadosVeiculos");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "DadosVeiculos",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_DadosVeiculos_Usuarios_UsuarioId",
                table: "DadosVeiculos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DadosVeiculos_Usuarios_UsuarioId",
                table: "DadosVeiculos");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "DadosVeiculos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DadosVeiculos_Usuarios_UsuarioId",
                table: "DadosVeiculos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
