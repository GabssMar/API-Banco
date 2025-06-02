using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoDsin.Migrations
{
    /// <inheritdoc />
    public partial class SegundaMigracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DadosVeiculos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Placa = table.Column<string>(type: "TEXT", nullable: false),
                    Modelo = table.Column<string>(type: "TEXT", maxLength: 7, nullable: false),
                    Fabricante = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    Cor = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Ano = table.Column<string>(type: "TEXT", nullable: false),
                    IdUsuario = table.Column<int>(type: "INTEGER", nullable: false),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DadosVeiculos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DadosVeiculos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Anexos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Evidencia = table.Column<string>(type: "TEXT", nullable: false),
                    Comentarios = table.Column<string>(type: "TEXT", nullable: false),
                    IdDadosVeiculo = table.Column<int>(type: "INTEGER", nullable: false),
                    DadosVeiculoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anexos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Anexos_DadosVeiculos_DadosVeiculoId",
                        column: x => x.DadosVeiculoId,
                        principalTable: "DadosVeiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DadosProprietarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Cnh = table.Column<string>(type: "TEXT", nullable: false),
                    Cpf = table.Column<string>(type: "TEXT", nullable: false),
                    IdDadosVeiculo = table.Column<int>(type: "INTEGER", nullable: false),
                    DadosVeiculoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DadosProprietarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DadosProprietarios_DadosVeiculos_DadosVeiculoId",
                        column: x => x.DadosVeiculoId,
                        principalTable: "DadosVeiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetalhesInfracaos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TipoInfracao = table.Column<string>(type: "TEXT", nullable: false),
                    CodigoInfracao = table.Column<string>(type: "TEXT", nullable: false),
                    LocaInfracao = table.Column<string>(type: "TEXT", nullable: false),
                    Data = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Hota = table.Column<string>(type: "TEXT", nullable: false),
                    Gravidade = table.Column<string>(type: "TEXT", nullable: false),
                    PontosCnh = table.Column<string>(type: "TEXT", nullable: false),
                    IdDadosVeiculo = table.Column<int>(type: "INTEGER", nullable: false),
                    DadosVeiculoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalhesInfracaos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalhesInfracaos_DadosVeiculos_DadosVeiculoId",
                        column: x => x.DadosVeiculoId,
                        principalTable: "DadosVeiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Anexos_DadosVeiculoId",
                table: "Anexos",
                column: "DadosVeiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_DadosProprietarios_DadosVeiculoId",
                table: "DadosProprietarios",
                column: "DadosVeiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_DadosVeiculos_UsuarioId",
                table: "DadosVeiculos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalhesInfracaos_DadosVeiculoId",
                table: "DetalhesInfracaos",
                column: "DadosVeiculoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Anexos");

            migrationBuilder.DropTable(
                name: "DadosProprietarios");

            migrationBuilder.DropTable(
                name: "DetalhesInfracaos");

            migrationBuilder.DropTable(
                name: "DadosVeiculos");
        }
    }
}
