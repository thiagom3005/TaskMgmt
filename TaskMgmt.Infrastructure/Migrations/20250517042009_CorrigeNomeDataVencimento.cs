using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskMgmt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrigeNomeDataVencimento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataVencimwento",
                table: "Tarefas",
                newName: "DataVencimento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataVencimento",
                table: "Tarefas",
                newName: "DataVencimwento");
        }
    }
}
