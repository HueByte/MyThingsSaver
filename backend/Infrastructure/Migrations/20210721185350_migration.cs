using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "Categories",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "CategoriesEntries",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Categories",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesEntries_OwnerId",
                table: "CategoriesEntries",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_OwnerId",
                table: "Categories",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_OwnerId",
                table: "Categories",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriesEntries_AspNetUsers_OwnerId",
                table: "CategoriesEntries",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_OwnerId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoriesEntries_AspNetUsers_OwnerId",
                table: "CategoriesEntries");

            migrationBuilder.DropIndex(
                name: "IX_CategoriesEntries_OwnerId",
                table: "CategoriesEntries");

            migrationBuilder.DropIndex(
                name: "IX_Categories_OwnerId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Categories",
                newName: "name");

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                table: "CategoriesEntries",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
