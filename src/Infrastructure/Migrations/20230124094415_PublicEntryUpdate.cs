using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PublicEntryUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PublicEntries_Entries_EntryId",
                table: "PublicEntries");

            migrationBuilder.DropIndex(
                name: "IX_PublicEntries_EntryId",
                table: "PublicEntries");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_PublicEntryId",
                table: "Entries",
                column: "PublicEntryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_PublicEntries_PublicEntryId",
                table: "Entries",
                column: "PublicEntryId",
                principalTable: "PublicEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_PublicEntries_PublicEntryId",
                table: "Entries");

            migrationBuilder.DropIndex(
                name: "IX_Entries_PublicEntryId",
                table: "Entries");

            migrationBuilder.CreateIndex(
                name: "IX_PublicEntries_EntryId",
                table: "PublicEntries",
                column: "EntryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PublicEntries_Entries_EntryId",
                table: "PublicEntries",
                column: "EntryId",
                principalTable: "Entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
