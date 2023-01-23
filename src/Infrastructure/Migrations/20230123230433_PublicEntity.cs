using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PublicEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Entries");

            migrationBuilder.AddColumn<int>(
                name: "PublicEntryId",
                table: "Entries",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PublicEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EntryId = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublicEntries_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PublicEntries_Entries_EntryId",
                        column: x => x.EntryId,
                        principalTable: "Entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PublicEntries_EntryId",
                table: "PublicEntries",
                column: "EntryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PublicEntries_UserId",
                table: "PublicEntries",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicEntries");

            migrationBuilder.DropColumn(
                name: "PublicEntryId",
                table: "Entries");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Entries",
                type: "BLOB",
                nullable: true);
        }
    }
}
