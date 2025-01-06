using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    public partial class CuisineNewTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCuisines");

            migrationBuilder.RenameColumn(
                name: "ProductCuisineId",
                table: "Products",
                newName: "CuisineId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CuisineId",
                table: "Products",
                column: "CuisineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Cuisines_CuisineId",
                table: "Products",
                column: "CuisineId",
                principalTable: "Cuisines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Cuisines_CuisineId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CuisineId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "CuisineId",
                table: "Products",
                newName: "ProductCuisineId");

            migrationBuilder.CreateTable(
                name: "ProductCuisines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CuisineId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Percentage = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCuisines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCuisines_Cuisines_CuisineId",
                        column: x => x.CuisineId,
                        principalTable: "Cuisines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCuisines_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCuisines_CuisineId",
                table: "ProductCuisines",
                column: "CuisineId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCuisines_ProductId",
                table: "ProductCuisines",
                column: "ProductId",
                unique: true);
        }
    }
}
