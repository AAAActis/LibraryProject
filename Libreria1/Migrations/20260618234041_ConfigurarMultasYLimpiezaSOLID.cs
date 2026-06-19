using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Libreria1.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurarMultasYLimpiezaSOLID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Multas_Prestamos_PrestamoAsignadoId",
                table: "Multas");

            migrationBuilder.DropForeignKey(
                name: "FK_Prestamos_Libros_LibroPrestadoIsbn",
                table: "Prestamos");

            migrationBuilder.DropForeignKey(
                name: "FK_Prestamos_Usuarios_UsuarioAsignadoId",
                table: "Prestamos");

            migrationBuilder.DropIndex(
                name: "IX_Prestamos_LibroPrestadoIsbn",
                table: "Prestamos");

            migrationBuilder.DropColumn(
                name: "LibroPrestadoIsbn",
                table: "Prestamos");

            migrationBuilder.RenameColumn(
                name: "UsuarioAsignadoId",
                table: "Prestamos",
                newName: "UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Prestamos_UsuarioAsignadoId",
                table: "Prestamos",
                newName: "IX_Prestamos_UsuarioId");

            migrationBuilder.RenameColumn(
                name: "PrestamoAsignadoId",
                table: "Multas",
                newName: "PrestamoId");

            migrationBuilder.RenameIndex(
                name: "IX_Multas_PrestamoAsignadoId",
                table: "Multas",
                newName: "IX_Multas_PrestamoId");

            migrationBuilder.AddColumn<string>(
                name: "LibroId",
                table: "Prestamos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_LibroId",
                table: "Prestamos",
                column: "LibroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Multas_Prestamos_PrestamoId",
                table: "Multas",
                column: "PrestamoId",
                principalTable: "Prestamos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prestamos_Libros_LibroId",
                table: "Prestamos",
                column: "LibroId",
                principalTable: "Libros",
                principalColumn: "Isbn",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prestamos_Usuarios_UsuarioId",
                table: "Prestamos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Multas_Prestamos_PrestamoId",
                table: "Multas");

            migrationBuilder.DropForeignKey(
                name: "FK_Prestamos_Libros_LibroId",
                table: "Prestamos");

            migrationBuilder.DropForeignKey(
                name: "FK_Prestamos_Usuarios_UsuarioId",
                table: "Prestamos");

            migrationBuilder.DropIndex(
                name: "IX_Prestamos_LibroId",
                table: "Prestamos");

            migrationBuilder.DropColumn(
                name: "LibroId",
                table: "Prestamos");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Prestamos",
                newName: "UsuarioAsignadoId");

            migrationBuilder.RenameIndex(
                name: "IX_Prestamos_UsuarioId",
                table: "Prestamos",
                newName: "IX_Prestamos_UsuarioAsignadoId");

            migrationBuilder.RenameColumn(
                name: "PrestamoId",
                table: "Multas",
                newName: "PrestamoAsignadoId");

            migrationBuilder.RenameIndex(
                name: "IX_Multas_PrestamoId",
                table: "Multas",
                newName: "IX_Multas_PrestamoAsignadoId");

            migrationBuilder.AddColumn<string>(
                name: "LibroPrestadoIsbn",
                table: "Prestamos",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_LibroPrestadoIsbn",
                table: "Prestamos",
                column: "LibroPrestadoIsbn");

            migrationBuilder.AddForeignKey(
                name: "FK_Multas_Prestamos_PrestamoAsignadoId",
                table: "Multas",
                column: "PrestamoAsignadoId",
                principalTable: "Prestamos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prestamos_Libros_LibroPrestadoIsbn",
                table: "Prestamos",
                column: "LibroPrestadoIsbn",
                principalTable: "Libros",
                principalColumn: "Isbn");

            migrationBuilder.AddForeignKey(
                name: "FK_Prestamos_Usuarios_UsuarioAsignadoId",
                table: "Prestamos",
                column: "UsuarioAsignadoId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
