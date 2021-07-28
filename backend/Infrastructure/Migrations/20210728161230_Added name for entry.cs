using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class Addednameforentry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ff01fee-8e4b-4d08-8e72-a6a2a0ec940c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d60c8dd4-45ea-49cb-a356-9851c0e897c4");

            migrationBuilder.AddColumn<string>(
                name: "CategoryEntryName",
                table: "CategoriesEntries",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "96b2d5a1-3256-4181-a93f-f0e14a2c51e3", "fdd76e20-aecf-49af-aa9a-b9190e751f89", "User", "user" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f5bdd284-d013-49e1-8df7-426032edcea0", "299d8556-7d0c-46d1-b664-0272731a2bc5", "Admin", "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "96b2d5a1-3256-4181-a93f-f0e14a2c51e3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f5bdd284-d013-49e1-8df7-426032edcea0");

            migrationBuilder.DropColumn(
                name: "CategoryEntryName",
                table: "CategoriesEntries");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d60c8dd4-45ea-49cb-a356-9851c0e897c4", "5681a9bd-9ddf-4d0b-bcaf-55e77075e4a9", "User", "user" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8ff01fee-8e4b-4d08-8e72-a6a2a0ec940c", "8c5c73fb-9297-4c25-9402-fc26bf4308a3", "Admin", "admin" });
        }
    }
}
