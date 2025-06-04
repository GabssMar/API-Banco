using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoDsin.Migrations
{
    /// <inheritdoc />
    public partial class AlterarEvidenciaParaByteArray : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LocaInfracao",
                table: "DetalhesInfracaos",
                newName: "LocalInfracao");

            migrationBuilder.RenameColumn(
                name: "Hota",
                table: "DetalhesInfracaos",
                newName: "Hora");

            migrationBuilder.AlterColumn<int>(
                name: "PontosCnh",
                table: "DetalhesInfracaos",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "Ano",
                table: "DadosVeiculos",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Evidencia",
                table: "Anexos",
                type: "BLOB",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LocalInfracao",
                table: "DetalhesInfracaos",
                newName: "LocaInfracao");

            migrationBuilder.RenameColumn(
                name: "Hora",
                table: "DetalhesInfracaos",
                newName: "Hota");

            migrationBuilder.AlterColumn<string>(
                name: "PontosCnh",
                table: "DetalhesInfracaos",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Ano",
                table: "DadosVeiculos",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Evidencia",
                table: "Anexos",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "BLOB");
        }
    }
}
