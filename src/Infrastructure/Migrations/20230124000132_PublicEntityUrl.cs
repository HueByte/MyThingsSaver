using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PublicEntityUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicUrl",
                table: "PublicEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicUrl",
                table: "PublicEntries");
        }
    }
}
