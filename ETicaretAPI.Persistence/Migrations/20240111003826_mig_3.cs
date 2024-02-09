using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETicaretAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "width",
                table: "FileEntities");

            migrationBuilder.AddColumn<bool>(
                name: "Showcase",
                table: "FileEntities",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Showcase",
                table: "FileEntities");

            migrationBuilder.AddColumn<int>(
                name: "width",
                table: "FileEntities",
                type: "integer",
                nullable: true);
        }
    }
}
