using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app_movie_mvc.Migrations
{
    /// <inheritdoc />
    public partial class TablaFavoritos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Favoritos_UsuarioId",
                table: "Favoritos");

            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "Favoritos",
                newName: "FechaAgregado");

            migrationBuilder.CreateIndex(
                name: "IX_Favoritos_UsuarioId_PeliculaId",
                table: "Favoritos",
                columns: new[] { "UsuarioId", "PeliculaId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Favoritos_UsuarioId_PeliculaId",
                table: "Favoritos");

            migrationBuilder.RenameColumn(
                name: "FechaAgregado",
                table: "Favoritos",
                newName: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_Favoritos_UsuarioId",
                table: "Favoritos",
                column: "UsuarioId");
        }
    }
}
